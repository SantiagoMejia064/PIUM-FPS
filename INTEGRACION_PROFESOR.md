# Integración del proyecto del profesor

- Destino: PIUM-FPS. Rama conservada: integracion-profe. Remoto original sin cambios. Sin commits ni push.
- Unity conservado: 6000.3.11f1. Packages/manifest.json y packages-lock.json conservados; no se instalaron paquetes.
- El proyecto del profesor se utilizó solo para lectura. Sus archivos de Assets se compararon por SHA-256 antes y después: sin cambios.

## Menú conservado

- Original: Assets/Scenes/MainMenu.unity; GUID a1aa4a5d6eaee87499457d9a3701576c.
- Respaldo independiente: Assets/Scenes/MainMenu_Respaldo.unity, con GUID diferente y contenido idéntico al original previo a la integración.
- Copia adicional con 62 dependencias originales y los archivos reemplazados: IntegrationBackup/2026-11-04/. Está fuera de Assets para evitar clases y GUID duplicados, y excluida de Git.
- Referencia del profesor: Assets/Scenes/Imported/MainMenu_Profesor.unity, GUID nuevo porque el del origen coincidía con SampleScene del destino. No se incluye en el Build.
- Conservados Canvas, CanvasScaler, cámaras, imágenes y RectTransforms originales. Se amplió únicamente la lista de hijos del Canvas para añadir selección de arena y resumen, reutilizando el estilo de tus botones.
- El selector original tenía arrays vacíos: ahora apunta a sus cuatro paneles/imágenes y a los personajes del profesor.
- Network, Opciones y Créditos conservan su interacción original habilitada, incluidos sus eventos originales vacíos para desarrollarlos más adelante. No se añadió un sistema online inexistente. Todos los componentes originales conservan sus estados Enabled/Interactable/Active.

## Sistemas integrados y adaptaciones

- Movimiento PlayerMovement, PlayerInput, cuatro prefabs de jugador, pistola, fábrica de armas y estados de disparo/recarga.
- Selección de personajes y mapas, MatchConfiguration, StartGameController, SummaryUI, spawns y pantalla dividida local para 2–4 jugadores.
- Reemplazados NIS.cs/NIS.inputactions, PlayerSelectionManager.cs y MenuNavigation.cs; conservados sus GUID originales. Se remapearon solo las referencias importadas, sin duplicar clases.
- SplitSrceenConfiguration.cs se importó como SplitScreenConfiguration.cs para coincidir con la clase MonoBehaviour, conservando su GUID de origen.
- Añadido el componente de pantalla dividida que faltaba en Arena2, la capa Weapon (6), y sincronización del viewport de cámaras de arma.
- Selección y spawn conservan la asociación de cada mando o teclado/ratón con su jugador. Los AudioListener se limitan al primero.
- MainMenuIntegration conecta tus botones con personajes, mapas y partida. Se conserva la navegación original: no desactiva move/submit del EventSystem ni MenuNavigation del selector.
- Se anuló una referencia obsoleta a un mesh inexistente en Smoke_Small_Light_FX (partícula billboard que no utiliza mesh). Respaldo previo conservado.
- Se reutilizaron recursos existentes por GUID; no se copiaron indiscriminadamente Assets ni ProjectSettings.

## Escenas habilitadas

1. Assets/Scenes/MainMenu.unity
2. Assets/Scenes/Arena1.unity
3. Assets/Scenes/Arena2.unity
4. Assets/Scenes/Loading.unity

SampleScene se conserva en disco. MainMenu_Respaldo y MainMenu_Profesor quedan fuera del Build. Loading se conserva como escena disponible; el flujo del profesor carga la arena directamente, no implementa progreso de carga.

## Verificación y límites

- Compilación directa de código de juego y Editor con Roslyn y referencias existentes de Unity 6000.3.11f1: correcta, sin errores (salida 0). Mensaje informativo del generador USG0001 al compilar fuera del proceso de Unity.
- Cero GUID duplicados en Assets, cero referencias locales fileID inexistentes en escenas/prefabs, cero referencias GUID sin resolver en las 109 dependencias transitivas del flujo integrado, cero scripts sin GUID resoluble en dicho flujo. Los métodos conectados existen. Network, Opciones y Créditos están habilitados con sus eventos originalmente vacíos.
- Comparación de bloques visuales originales y GUID del menú: preservados.
- Hay referencias heredadas no resueltas en recursos de demostración fuera del flujo integrado (TextMesh Pro antiguo y otros recursos). El informe static-validation.json las enumera, incluidas 166 referencias m_Script de ejemplos de TextMesh Pro que ya estaban presentes. No se afirma que todo el repositorio esté libre de incidencias heredadas.
- Unity en batch salió con código 1 antes de importar assets y ejecutar el validador, incluso tras conectar la licencia. La revisión de objetos dentro de Unity no pudo ejecutarse; no se entró en Play Mode.
- Puedes ejecutar Tools > PIUM > Validate Integration dentro del editor. Guarda tus escenas antes: abre las escenas para inspeccionarlas y restaura la configuración al terminar. El informe aparece en IntegrationReports/unity-validation.txt.

## Comprobación manual al abrir Unity

1. Abrir con 6000.3.11f1, esperar la importación y revisar Console/Build Profiles. MainMenu debe ser la primera escena habilitada.
2. En MainMenu: Inicio > Local. Entrar con Enter o A/Sur del mando; elegir con A/D o stick/d-pad horizontal; confirmar con Enter o A/Sur. Cancelar con Esc o B/Este.
3. Se requieren 2–4 jugadores y personajes distintos: un teclado/ratón y uno o más mandos, o varios mandos. Tras confirmar todos, usar el botón Continuar mediante la navegación original o el ratón.
4. Elegir Arena 1 o Arena 2, revisar mapa/jugadores en el resumen y activar el botón Iniciar partida. Probar también Volver.
5. Verificar que cada dispositivo controla solo su jugador, los puntos de spawn, la división para 2/3/4 jugadores, cámara y arma, disparo/recarga y un solo AudioListener. Probar ambas arenas.
6. Comparar tu diseño con MainMenu_Respaldo. Las pruebas jugables quedan a cargo del usuario según lo solicitado.
