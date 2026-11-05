# Iluminación de MainMenu

La escena modificada es Assets/Scenes/MainMenu.unity. Se conserva su GUID, interfaz, interacción y personaje. El respaldo previo a iluminación está en IntegrationBackup/Iluminacion_Menu/MainMenu_antes_iluminacion.unity.

En Hierarchy encontrarás Iluminacion_Menu:
- Luz_Personaje_Principal: Spot cálido desde arriba y delante; intensidad 180, alcance 48, cono 65°, sombras suaves.
- Luz_Personaje_Contorno: Spot frío detrás del personaje; intensidad 110, alcance 48, sin sombras.
- PostProcesado_Menu: Volume global con prioridad 10 y Weight 1.

Los valores de las luces consideran el personaje ampliado 9,5 veces. Modifica Intensity, Range, Spot Angle, Color y Transform desde cada componente Light. La Directional Light existente queda en intensidad 0,85.

Para trabajar el postprocesado, selecciona Iluminacion_Menu > PostProcesado_Menu y abre su Profile. El asset es Assets/Settings/MainMenu_PostProcesado.asset, independiente de los otros perfiles del proyecto.

Perfil ampliado con las 19 opciones compatibles del Default: Lift Gamma Gain, Split Toning, Motion Blur, Color Adjustments, Film Grain, Tonemapping, Panini Projection, Bloom, White Balance, Screen Space Lens Flare, Shadows Midtones Highlights, Color Curves, Color Lookup, Depth Of Field, Chromatic Aberration, Vignette, Lens Distortion, Channel Mixer y Probe Volumes Options. Los componentes de prueba del Default con scripts inexistentes no se copiaron. El perfil Default original no se modificó.

Ambiente de combate suave:
- Saturation -16, Contrast +12, Post Exposure -0,1; White Balance Temperature -7 y Tint -2.
- Split Toning: sombras frías y luces ligeramente cálidas. Tonemapping ACES.
- Bloom 0,12; Vignette 0,18; Film Grain 0,06.
- Depth Of Field Gaussian: Start 70, End 220 y Max Radius 0,65. Solo desenfoca la distancia; el personaje y el Canvas están más cerca. Si quieres menos desenfoque, baja Max Radius o desactiva el override Mode.
- Motion Blur, aberración cromática, distorsión, Panini, LUT y destellos conservan sus opciones visibles con efecto neutro.
- Niebla atmosférica lineal, inicio 180 y final 1500, gris con un matiz de polvo. Se ajusta fuera del Volume en Window > Rendering > Lighting > Environment > Other Settings > Fog. El nombre o ubicación visual puede variar dentro de la pestaña Environment.

Para comparar todo el estilo con la niebla original desactivada, cambia Weight del Volume a 0 y desmarca Fog en Lighting. El respaldo previo a este ajuste está en IntegrationBackup/PostProcesado_Combate/.

Puedes añadir efectos desde Add Override > Post-processing. Activa la casilla junto a cada parámetro que quieras modificar. Para comparar, cambia Weight del Volume entre 0 y 1.

Main Camera tiene Rendering > Post Processing activado; su máscara de volúmenes incluye Default, donde está el Volume. En Scene View activa también la visualización de Post Processing si deseas previsualizarlo allí.

No se instalaron paquetes, no se modificaron los controles ni se entró en Play Mode. Se verificaron referencias locales, perfil y preservación de la interfaz por comparación de archivos. El resultado visual está pendiente de revisar en Unity; ajustar las intensidades según Game View.
