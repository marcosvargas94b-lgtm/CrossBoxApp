
    // --- VARIABLES GLOBALES ---
    let timerInterval = null;
    let totalSeconds = 0;
    let isRunning = false;

    // Variables para calcular tiempo transcurrido
    let initialSecondsForCalc = 0; // Guardará el tiempo inicial para hacer la resta

    // Configuración Timer
    let timerMode = 'countdown';
    let roundsSettings = {total: 10, mins: 1, current: 1 };

    // Configuración Atletas
    let athleteMode = 'TIMER ATLETAS';
    let selectedAthleteId = null;

    // --- FUNCIONES UTILIDAD ---
    function closeModal(id) {document.getElementById(id).style.display = 'none'; }
    function openModal(id) {document.getElementById(id).style.display = 'flex'; }

    // Helper para formatear segundos a MM:SS
    function formatTime(sec) {
        const m = Math.floor(sec / 60).toString().padStart(2, '0');
    const s = (sec % 60).toString().padStart(2, '0');
    return `${m}:${s}`;
    }

    // --- 1. LÓGICA WORKOUT (IZQUIERDA) ---
    let editingBlock = null;

    function openWorkoutModal(block = null) {
        const container = document.getElementById('workoutInputs');
    const titleInput = document.getElementById('inputBlockTitle'); // Nuevo input
    container.innerHTML = '';
    editingBlock = block;

    if(block) {
            // Cargar datos existentes si es edición
            // Recuperamos el título del h4 que está dentro del bloque
            const titleEl = block.querySelector('.block-title');
    titleInput.value = titleEl ? titleEl.innerText : '';

    const rows = block.querySelectorAll('.workout-row');
            rows.forEach(r => {
                const q = r.querySelector('.w-qty').innerText.replace('• ', '');
    const n = r.querySelector('.w-name').innerText;
    addInputRow(q, n);
            });
        } else {
        // Limpiar y Filas por defecto
        titleInput.value = '';
    addInputRow(); addInputRow(); addInputRow();
        }
    openModal('modalWorkout');
    }

    function addInputRow(q = '', n = '') {
        const div = document.createElement('div');
    div.className = 'row-inputs';
    div.innerHTML = `<input type="text" class="i-qty" placeholder="Cant" value="${q}"><input type="text" class="i-name" placeholder="Ejercicio" value="${n}">`;
        document.getElementById('workoutInputs').appendChild(div);
    }

        function saveWorkoutBlock() {
        const inputs = document.querySelectorAll('#workoutInputs .row-inputs');
        const titleVal = document.getElementById('inputBlockTitle').value.toUpperCase(); // Tomar valor título
        let html = '';

        // --- NUEVO: Agregar Título al HTML del bloque ---
        if(titleVal) {
            html += `<h4 class="block-title" style="margin:0 0 10px 0; color:#FDF5D3; border-bottom:1px solid #555; padding-bottom:5px;">${titleVal}</h4>`;
        }

        inputs.forEach(row => {
            const q = row.querySelector('.i-qty').value;
        const n = row.querySelector('.i-name').value;
        if(n) html += `<div class="workout-row"><span class="w-qty">• ${q}</span><span class="w-name">${n}</span></div>`;
        });

        const controls = `<div class="block-controls">
            <button class="icon-btn" onclick="openWorkoutModal(this.parentElement.parentElement)"><i class="fas fa-edit"></i></button>
            <button class="icon-btn" onclick="this.parentElement.parentElement.remove()"><i class="fas fa-trash"></i></button>
        </div>`;

        if(editingBlock) {
            editingBlock.innerHTML = html + controls;
        } else {
            const div = document.createElement('div');
        div.className = 'black-block';
        // Aplicamos un estilo extra al bloque para asegurar que el texto blanco se vea bien
        div.style.color = "#FDF5D3";
        div.style.backgroundColor = "#1a1a1a"; // Asegurar fondo oscuro para contraste
        div.innerHTML = html + controls;
        document.getElementById('workoutList').appendChild(div);
        }
        closeModal('modalWorkout');
    }

        // --- 2. LÓGICA TIMER (DERECHA SUPERIOR) ---
        function openTimerSettings() {openModal('modalTimer'); }

        function toggleTimerOptions() {
        const type = document.getElementById('timerType').value;
        document.getElementById('optCountdown').style.display = type === 'countdown' ? 'block' : 'none';
        document.getElementById('optRounds').style.display = type === 'rounds' ? 'block' : 'none';
    }

        function applyTimerSettings() {
            stopTimer();
        timerMode = document.getElementById('timerType').value;

        if(timerMode === 'countdown') {
            const mins = parseInt(document.getElementById('inputTotalMins').value);
        totalSeconds = mins * 60;
        initialSecondsForCalc = totalSeconds; // Guardamos el total inicial para calcular transcurrido
        document.getElementById('roundIndicator').style.display = 'none';
        } else {
            roundsSettings.total = parseInt(document.getElementById('inputRounds').value);
        roundsSettings.mins = parseInt(document.getElementById('inputMinsPerRound').value);
        roundsSettings.current = 1;
        totalSeconds = roundsSettings.mins * 60;
        initialSecondsForCalc = totalSeconds; // Guardamos tiempo por ronda

        document.getElementById('roundIndicator').style.display = 'block';
        document.getElementById('totalRounds').innerText = roundsSettings.total;
        document.getElementById('currentRound').innerText = 1;
        }
        updateTimerDisplay(totalSeconds);
        closeModal('modalTimer');
    }

        function updateTimerDisplay(seconds) {
            document.getElementById('displayTime').innerText = formatTime(seconds);
    }

        function toggleTimer() {
        if(isRunning) stopTimer();
        else startTimer();
    }

        function startTimer() {
            isRunning = true;
        document.getElementById('playPauseBtn').className = "fas fa-pause-circle fa-3x";
        
        timerInterval = setInterval(() => {
            if(totalSeconds > 0) {
            totalSeconds--;
        updateTimerDisplay(totalSeconds);
            } else {
                // Timer llegó a 0
                if(timerMode === 'rounds') {
                    if(roundsSettings.current < roundsSettings.total) {
            roundsSettings.current++;
        document.getElementById('currentRound').innerText = roundsSettings.current;
        totalSeconds = roundsSettings.mins * 60; // Reiniciar
        initialSecondsForCalc = totalSeconds; // Reiniciar referencia para cálculo
                    } else {
            stopTimer(); 
                    }
                } else {
            stopTimer(); 
                }
            }
        }, 1000);
    }

        function stopTimer() {
            isRunning = false;
        clearInterval(timerInterval);
        document.getElementById('playPauseBtn').className = "fas fa-play-circle fa-3x";
    }

        // --- 3. LÓGICA ATLETAS (DERECHA INFERIOR) ---
        function openAthleteTitleModal() {openModal('modalTitle'); }

        function setAthleteMode(mode) {
            athleteMode = mode;
        document.getElementById('athleteSectionTitle').innerText = mode;
        closeModal('modalTitle');

        const rows = document.querySelectorAll('.athlete-time');
        rows.forEach(div => {
            div.innerText = (mode === 'RONDAS ATLETAS') ? "0" : document.getElementById('displayTime').innerText;
        div.style.color = "#333"; 
        });
    }

        function openAddAthletesModal() {openModal('modalAthletes'); }

        function saveAthletes() {
        const text = document.getElementById('inputAthleteNames').value;
        const names = text.split('\n');
        const list = document.getElementById('athleteList');

        names.forEach(name => {
            if(name.trim()) {
                const li = document.createElement('li');
        li.className = 'athlete-row';
        li.id = 'athlete-' + Math.random().toString(36).substr(2, 9);
        const initialVal = (athleteMode === 'RONDAS ATLETAS') ? "0" : "00:00";

        li.innerHTML = `
        <div class="athlete-time">${initialVal}</div>
        <div class="athlete-name">${name}</div>
        <div class="athlete-btn" onclick="handleAthleteAction('${li.id}')">
            <i class="fas fa-chevron-right"></i>
        </div>
        `;
        list.appendChild(li);
            }
        });
        document.getElementById('inputAthleteNames').value = '';
        closeModal('modalAthletes');
    }

        function handleAthleteAction(id) {
        const row = document.getElementById(id);
        const timeDiv = row.querySelector('.athlete-time');
        const btn = row.querySelector('.athlete-btn');

        if(athleteMode === 'TIMER ATLETAS') {
            // --- NUEVO LÓGICA: Calcular Tiempo Transcurrido ---
            // Fórmula: Tiempo Inicial - Tiempo Actual = Tiempo Transcurrido
            const elapsedSeconds = initialSecondsForCalc - totalSeconds;

        // Usamos la función formatTime para que se vea bonito (ej: 01:15)
        timeDiv.innerText = formatTime(elapsedSeconds);

        timeDiv.style.color = "#d9534f";
        btn.style.background = "#333";
        btn.style.color = "#fff";
        }
        else if (athleteMode === 'RONDAS ATLETAS') {
            selectedAthleteId = id;
        document.getElementById('manualScoreInput').value = '';
        openModal('modalScore');
        }
    }

        function saveManualScore() {
        const val = document.getElementById('manualScoreInput').value;
        const row = document.getElementById(selectedAthleteId);
        if(row) {
            row.querySelector('.athlete-time').innerText = val;
        row.querySelector('.athlete-btn').style.background = "#333";
        row.querySelector('.athlete-btn').style.color = "#fff";
        }
        closeModal('modalScore');
    }
