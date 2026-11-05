# Actualización del profesor 02

Se integraron las novedades de la misma carpeta fuente en PIUM-FPS, sobre la rama integracion-profe (base d8207b8). No se creó commit ni se hizo push.

## Conservación

MainMenu.unity y su GUID, Canvas, botones, controles, luces y postprocesado permanecen idénticos por SHA-256 al estado previo a esta actualización. También se preservaron MainMenuIntegration, PlayerSelectionManager adaptado, MatchConfiguration con dispositivos y SplitScreenConfiguration con viewports de armas. No se sustituyó el menú por el del profesor. Su escena de referencia se mantiene en Assets/Scenes/Imported/MainMenu_Profesor.unity.

Unity sigue en 6000.3.11f1. Packages, ProjectVersion, Build Settings y TagManager no se modificaron. El proyecto fuente no se modificó.

Respaldo de los archivos anteriores y manifiesto de importación: IntegrationBackup/Actualizacion_Profesor_02/, fuera de Assets y excluido de Git. No se eliminó la versión anterior de la pistola: está respaldada allí.

## Contenido añadido y actualizado

- Assets/Prefabs/Armas/: 14 prefabs organizados como en el profesor: Asalto, Corto Alcance, Lanzar, Pistolas, Shotgun y Sniper. Los modelos y materiales Polygon ya existentes se reutilizan por GUID.
- Pistola pasó de Assets/Prefabs/Armas/Pistola.prefab a Assets/Prefabs/Armas/Pistolas/Pistola.prefab conservando GUID y referencias. El origen contiene dos pistolas distintas con el mismo GUID; se eligió la de Pistolas, con PistolFlash configurado.
- Assets/Scriptable Objects/Armas/Pistola.asset y Assets/Scripts/Armas/Datos/DatosArmas.cs.
- Assets/Prefabs/FX/Pistola Flash.prefab y biblioteca Assets/JMO Assets/Cartoon FX Remaster/, incluidos shaders, materiales, texturas, scripts y demos. Las demos no se añadieron al Build.
- Scripts de armas actualizados: disparo desde la cámara, daño a jugadores, eventos de munición y efectos de disparo mediante object pool.
- Salud, estadísticas, respawn, HealthObserver, MatchManager, HUD y resultados en sus carpetas Scripts/Core, Scripts/Player y Scripts/UI/HUD.
- Prefab1–4 actualizados con salud/respawn/estadísticas/HUD. Arena1 y Arena2 actualizadas con MatchManager, pools, pantalla dividida y resultados.
- Dos materiales Polygon (Dirt y Rain) actualizados conservando sus shaders URP.

Se copiaron 524 archivos de recursos/código, más sus metadatos y carpetas. No se importaron archivos comprimidos de instalación antiguos ni la pantalla de bienvenida del proveedor; no son dependencias de los sistemas de juego.

## Adaptaciones

- Se conservaron el emparejamiento por jugador de teclado/ratón o mando, el bloqueo del cambio automático de dispositivo y un solo AudioListener activo.
- Se notifican cambios de munición también al terminar la recarga para refrescar el HUD.
- Se corrigieron identificadores serializados antiguos WeaponData → DatosArmas y MatchResultsUI → Resultados, manteniendo los GUID de sus scripts.
- No se copiaron scripts duplicados de UI/Menu del origen. Las copias son idénticas y tienen GUID duplicados; el destino conserva una sola clase por sistema.

## Verificación

Compilación directa con Roslyn/referencias de Unity 6000.3.11f1 correcta (código 0) para los siete ensamblados: CFXRRuntime, CFXREditor, KinoBloom.Runtime, KinoBloom.Editor, CFXRDemo, Assembly-CSharp y Assembly-CSharp-Editor. USG0001 es un mensaje informativo del generador al compilar fuera del proceso de Unity.

Auditoría de las 130 dependencias transitivas del flujo MainMenu, ambas arenas y datos de pistola: cero GUID sin resolver, cero scripts sin GUID resoluble, cero referencias locales fileID inexistentes y cero discrepancias de clase serializada. Cero GUID duplicados en Assets. Los archivos fuente usados se verificaron por SHA-256 sin cambios.

Informe completo: IntegrationReports/actualizacion-profesor-02.json. No se ejecutó Play Mode ni la importación/renderización de shaders dentro de Unity; queda esa comprobación en el editor.

## Limitaciones que ya trae el origen

- La biblioteca opcional Cartoon FX incluye 13 referencias a recursos ausentes también en el profesor (algunas texturas, sprites, mesh y configuración de iluminación de la demo). Están enumeradas en el informe. Ninguna está en las dependencias del flujo de juego/pistola integrado. No se inventaron sustitutos.
- Granada, Grenada Humo y Pistola Especial se importan como están: su firePoint no está asignado. Las granadas todavía usan Armas por raycast y no traen implementación de lanzamiento/explosión. No son armas completamente funcionales por el solo hecho de importar sus modelos.
- DatosArmas configura los pools de efectos. Sus valores numéricos todavía no se aplican a Armas: la implementación del profesor mantiene daño/cadencia/munición en cada prefab. Solo hay un asset de datos, Pistola, en el origen.
- La pistola sigue siendo el arma inicial; no se encontró selector/intercambio de todas las armas nuevas.

## Revisar en Unity

Abrir con 6000.3.11f1, dejar terminar la importación y revisar Console (incluidos shaders Cartoon FX). Iniciar desde tu MainMenu, unir y confirmar 2–4 jugadores, probar ambas arenas. Verificar que cada dispositivo controla su jugador, salud y daño, munición al disparar/recargar, flash de pistola, respawn de 3 segundos, muertes/eliminaciones y fin de partida (20 eliminaciones o 600 segundos). Probar Volver al menú y Repetir partida en resultados. Las otras armas se pueden inspeccionar desde sus carpetas; las limitaciones anteriores siguen pendientes de desarrollo.
