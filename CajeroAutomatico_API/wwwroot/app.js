// ── Utilidades ──────────────────────────────────────────────
const api = () => document.getElementById('apiBase').value.replace(/\/$/, '');
const v   = id  => document.getElementById(id)?.value ?? '';

function fmt(num) {
  return Number(num).toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}

// ── Mostrar respuesta ────────────────────────────────────────
function showResp(id, ok, data) {
  const el = document.getElementById(id);
  if (!el) return;
  el.className = 'resp show';
  let content = '';

  if (!ok) {
    content = `
      <div class="resp-head rh-err">✗ ERROR &nbsp;·&nbsp; ${new Date().toLocaleTimeString()}</div>
      <div class="resp-body">${JSON.stringify(data, null, 2)}</div>`;

  } else if (data.noTarjeta && data.titular !== undefined) {
    // Obtener tarjeta
    const no = data.noTarjeta.replace(/(.{4})/g, '$1 ').trim();
    const fv = new Date(data.fechaVencimiento);
    const mm = String(fv.getMonth() + 1).padStart(2, '0');
    const yy = fv.getFullYear();
    content = `
      <div class="resp-head rh-ok">✓ EXITOSO &nbsp;·&nbsp; ${new Date().toLocaleTimeString()}</div>
      <div style="padding:16px;background:#040810;">
        <div style="background:linear-gradient(135deg,#0d1f3c,#0a1628);border:1px solid #1a3050;border-radius:14px;padding:18px;max-width:440px;">
          <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:14px;padding-bottom:12px;border-bottom:1px solid #1a3050;">
            <span style="font-family:var(--mono);font-size:11px;color:var(--accent);letter-spacing:1px;">TARJETA DE DÉBITO</span>
            <span style="background:${data.activa?'#071a0f':'#1a070d'};color:${data.activa?'var(--green)':'var(--red)'};font-size:10px;padding:2px 10px;border-radius:12px;font-family:var(--mono);">
              ${data.activa ? '● ACTIVA' : '● INACTIVA'}
            </span>
          </div>
          <div style="font-family:var(--mono);font-size:19px;color:var(--text);letter-spacing:4px;margin-bottom:14px;">${no}</div>
          <div style="display:grid;grid-template-columns:1fr 1fr;gap:12px;margin-bottom:14px;">
            <div><div style="font-size:9px;color:var(--muted);text-transform:uppercase;letter-spacing:.6px;margin-bottom:3px;">Titular</div><div style="font-size:13px;font-weight:600;">${data.titular}</div></div>
            <div><div style="font-size:9px;color:var(--muted);text-transform:uppercase;letter-spacing:.6px;margin-bottom:3px;">Vencimiento</div><div style="font-size:13px;font-weight:600;">${mm}/${yy}</div></div>
            <div><div style="font-size:9px;color:var(--muted);text-transform:uppercase;letter-spacing:.6px;margin-bottom:3px;">CVV</div><div style="font-size:13px;font-weight:600;">${data.cvv}</div></div>
            <div><div style="font-size:9px;color:var(--muted);text-transform:uppercase;letter-spacing:.6px;margin-bottom:3px;">No. Cuenta</div><div style="font-size:13px;font-weight:600;">${data.noCuenta}</div></div>
          </div>
          <div style="background:#060a12;border:1px solid #1a3050;border-radius:10px;padding:12px 16px;display:flex;justify-content:space-between;align-items:center;">
            <span style="font-size:10px;color:var(--muted);text-transform:uppercase;letter-spacing:.6px;">Saldo disponible</span>
            <span style="font-family:var(--mono);font-size:22px;color:var(--green);">Q ${fmt(data.saldo)}</span>
          </div>
        </div>
      </div>`;

  } else if (data.hasOwnProperty('valido')) {
    // Validar PIN
    const pinOk = data.valido === 1 || data.valido === true;
    content = `
      <div class="resp-head ${pinOk ? 'rh-ok' : 'rh-err'}">${pinOk ? '✓ EXITOSO' : '✗ FALLIDO'} &nbsp;·&nbsp; ${new Date().toLocaleTimeString()}</div>
      <div style="padding:20px;background:#040810;text-align:center;">
        <div style="font-size:44px;margin-bottom:10px;">${pinOk ? '🔓' : '🔒'}</div>
        <div style="font-family:var(--mono);font-size:16px;color:${pinOk ? 'var(--green)' : 'var(--red)'};letter-spacing:2px;margin-bottom:6px;">${pinOk ? 'PIN CORRECTO' : 'PIN INCORRECTO'}</div>
        <div style="font-size:12px;color:var(--muted);">${data.mensaje}</div>
      </div>`;

  } else if (data.idTarjeta !== undefined && data.resultado !== undefined) {
    // Crear tarjeta
    content = `
      <div class="resp-head rh-ok">✓ EXITOSO &nbsp;·&nbsp; ${new Date().toLocaleTimeString()}</div>
      <div style="padding:16px;background:#040810;">
        <div style="background:#071a0f;border:1px solid #0a2a14;border-radius:12px;padding:20px;text-align:center;">
          <div style="font-size:36px;margin-bottom:10px;">💳</div>
          <div style="font-family:var(--mono);font-size:13px;color:var(--green);letter-spacing:1px;margin-bottom:14px;">TARJETA CREADA EXITOSAMENTE</div>
          <div style="background:#060a12;border-radius:8px;padding:10px 20px;display:inline-block;">
            <div style="font-size:10px;color:var(--muted);text-transform:uppercase;letter-spacing:.6px;">ID Tarjeta asignado</div>
            <div style="font-family:var(--mono);font-size:26px;color:var(--accent);margin-top:4px;">#${data.idTarjeta}</div>
          </div>
        </div>
      </div>`;

  } else if (data.idMovimiento !== undefined && data.saldoNuevo !== undefined) {
    // Depósito o Nota crédito
    const diff = Number(data.saldoNuevo) - Number(data.saldoAnterior);
    content = `
      <div class="resp-head rh-ok">✓ EXITOSO &nbsp;·&nbsp; ${new Date().toLocaleTimeString()}</div>
      <div style="padding:16px;background:#040810;">
        <div style="background:#071a0f;border:1px solid #0a2a14;border-radius:12px;padding:18px;">
          <div style="display:grid;grid-template-columns:1fr 1fr 1fr;gap:14px;text-align:center;margin-bottom:14px;">
            <div>
              <div style="font-size:9px;color:var(--muted);text-transform:uppercase;letter-spacing:.6px;margin-bottom:6px;">Saldo anterior</div>
              <div style="font-family:var(--mono);font-size:16px;color:var(--text);">Q ${fmt(data.saldoAnterior)}</div>
            </div>
            <div>
              <div style="font-size:9px;color:var(--muted);text-transform:uppercase;letter-spacing:.6px;margin-bottom:6px;">Monto acreditado</div>
              <div style="font-family:var(--mono);font-size:16px;color:var(--green);">+ Q ${fmt(diff)}</div>
            </div>
            <div>
              <div style="font-size:9px;color:var(--muted);text-transform:uppercase;letter-spacing:.6px;margin-bottom:6px;">Saldo nuevo</div>
              <div style="font-family:var(--mono);font-size:16px;color:var(--accent);">Q ${fmt(data.saldoNuevo)}</div>
            </div>
          </div>
          <div style="padding-top:12px;border-top:1px solid #1a3050;text-align:center;">
            <span style="font-size:10px;color:var(--muted);">ID Movimiento: </span>
            <span style="font-family:var(--mono);font-size:11px;color:var(--accent);">#${data.idMovimiento}</span>
            <span style="font-size:10px;color:var(--muted);margin-left:12px;">Estado: </span>
            <span style="background:#071a0f;color:var(--green);font-size:10px;padding:1px 8px;border-radius:10px;font-family:var(--mono);">${data.resultado}</span>
          </div>
        </div>
      </div>`;

  } else {
    // Fallback JSON para todo lo demás
    const isArr = Array.isArray(data);
    content = `
      <div class="resp-head rh-ok">✓ EXITOSO &nbsp;·&nbsp; ${new Date().toLocaleTimeString()}${isArr ? ` &nbsp;·&nbsp; ${data.length} registros` : ''}</div>
      <div class="resp-body">${JSON.stringify(data, null, 2)}</div>`;
  }

  el.innerHTML = content;
}

// ── Llamadas al API ──────────────────────────────────────────
async function call(method, endpoint, body, respId) {
  try {
    const res = await fetch(api() + endpoint, {
      method,
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(body)
    });
    const data = await res.json();
    showResp(respId, res.ok, data);
    return { ok: res.ok, data };
  } catch (e) {
    showResp(respId, false, { error: e.message });
    return { ok: false, data: { error: e.message } };
  }
}

async function callGet(endpoint, respId) {
  try {
    const res  = await fetch(api() + endpoint);
    const data = await res.json();
    showResp(respId, res.ok, data);
    return { ok: res.ok, data };
  } catch (e) {
    showResp(respId, false, { error: e.message });
    return { ok: false, data: { error: e.message } };
  }
}

// ── Navegación ───────────────────────────────────────────────
function go(name) {
  document.querySelectorAll('.panel').forEach(p =>
    p.classList.toggle('active', p.id === 'panel-' + name));
  document.querySelectorAll('.nav-btn').forEach(b =>
    b.classList.toggle('active', b.getAttribute('onclick')?.includes("'" + name + "'")));
}

// ── Bitácora ─────────────────────────────────────────────────
function cargarBitacora() {
  const idc = v('bit-idc'), fi = v('bit-fi'), ff = v('bit-ff');
  let url = `/api/bitacora/obtener?idCuenta=${idc}`;
  if (fi) url += `&fechaInicio=${fi}`;
  if (ff) url += `&fechaFin=${ff}`;
  callGet(url, 'resp-bit');
}

function cargarMovimientos() {
  const idc = v('mov-idc'), tipo = v('mov-tipo'), fi = v('mov-fi'), ff = v('mov-ff');
  let url = `/api/bitacora/movimientos?idCuenta=${idc}`;
  if (tipo) url += `&idTipoMovimiento=${tipo}`;
  if (fi)   url += `&fechaInicio=${fi}`;
  if (ff)   url += `&fechaFin=${ff}`;
  callGet(url, 'resp-mov');
}

// ── Concurrencia ─────────────────────────────────────────────
function limpiar() {
  document.getElementById('hilo-grid').innerHTML = '';
  document.getElementById('resp-sim').className  = 'resp';
  document.getElementById('stats-box').style.display = 'none';
}

async function simular() {
  const idCuenta = +v('cc-idc');
  const monto    = +v('cc-monto');
  const nHilos   = Math.min(+v('cc-hilos'), 20);
  const tipo     = v('cc-tipo');
  const delay    = +v('cc-delay');
  const grid     = document.getElementById('hilo-grid');
  const btn      = document.getElementById('btn-sim');

  btn.disabled = true;
  grid.innerHTML = '';
  document.getElementById('stats-box').style.display = 'grid';

  const tipos = ['deposito', 'notacredito', 'pagarcheque', 'notadebito'];

  const hilos = Array.from({ length: nHilos }, (_, i) => {
    let t;
    if      (tipo === 'deposito')    t = 'deposito';
    else if (tipo === 'notacredito') t = 'notacredito';
    else if (tipo === 'mixto')       t = i % 2 === 0 ? 'deposito' : 'notacredito';
    else                             t = tipos[i % tipos.length];

    const card = document.createElement('div');
    card.className = 'hilo-card';
    card.id = `hc-${i}`;
    card.innerHTML = `
      <div class="hilo-num">#${i + 1}</div>
      <div class="hilo-type">${t}</div>
      <div class="hilo-st st-p" id="hst-${i}">⏳ Pendiente</div>
      <div class="hilo-res" id="hres-${i}"></div>`;
    grid.appendChild(card);
    return { i, t };
  });

  document.getElementById('st-total').textContent = nHilos;
  document.getElementById('st-ok').textContent    = 0;
  document.getElementById('st-err').textContent   = 0;
  document.getElementById('st-ms').textContent    = '...';

  let okCount = 0, errCount = 0;
  const inicio = Date.now();

  const promesas = hilos.map(async h => {
    if (delay > 0) await new Promise(r => setTimeout(r, h.i * delay));

    const card = document.getElementById(`hc-${h.i}`);
    const st   = document.getElementById(`hst-${h.i}`);
    const res  = document.getElementById(`hres-${h.i}`);

    card.className = 'hilo-card running';
    st.className   = 'hilo-st st-r';
    st.textContent = '🔄 Ejecutando';

    const body = {
      idCuenta,
      monto,
      descripcion: `Hilo concurrente #${h.i + 1}`,
      usuario: 'simulador',
      ...(h.t === 'pagarcheque' ? { noCheque: `CHQ-SIM-${h.i}`, fechaCheque: new Date().toISOString().split('T')[0] } : {}),
      ...(h.t === 'notadebito'  ? { idTarjeta: 1 } : {})
    };

    try {
      const r    = await fetch(api() + `/api/transaccion/${h.t}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
      });
      const data = await r.json();

      if (r.ok) {
        card.className = 'hilo-card ok';
        st.className   = 'hilo-st st-ok';
        st.textContent = '✓ Exitoso';
        res.textContent= `Saldo: Q${fmt(data.saldoNuevo ?? data.SaldoNuevo ?? 0)}`;
        okCount++;
      } else {
        card.className = 'hilo-card error';
        st.className   = 'hilo-st st-er';
        st.textContent = '✗ Error';
        res.textContent= data.error ?? data.Error ?? 'Error';
        errCount++;
      }
    } catch (e) {
      card.className = 'hilo-card error';
      st.className   = 'hilo-st st-er';
      st.textContent = '✗ Sin conexión';
      errCount++;
    }

    document.getElementById('st-ok').textContent  = okCount;
    document.getElementById('st-err').textContent = errCount;
  });

  await Promise.all(promesas);
  const ms = Date.now() - inicio;
  document.getElementById('st-ms').textContent = ms + 'ms';

  showResp('resp-sim', errCount === 0, {
    resumen:    `${okCount}/${nHilos} hilos exitosos`,
    tiempoTotal:`${ms}ms`,
    conclusion: okCount === nHilos
      ? '✓ Control de concurrencia correcto. Ningún hilo causó inconsistencia.'
      : `⚠ ${errCount} hilo(s) fallaron. Revisar bitácora para ver los errores registrados.`
  });

  btn.disabled = false;
}
