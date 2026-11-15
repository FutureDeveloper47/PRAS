# Ideas que no implementé (necesitan cambiar casos de uso o el modelo)

Estas ideas salieron mientras hacía las mejoras de interfaz del 15/11 (rama `mejoras-ui-15-11`). No las hice porque el prompt pide no agregar casos de uso, tablas ni columnas: el modelo de Enterprise Architect ya está cerrado.

1. **Las 7 pantallas del módulo Taller filtradas por el mecánico que eligió el dueño.**
   Hoy, desde el panel MECÁNICOS del dueño, se puede *consultar* el trabajo de un mecánico: sus órdenes, sus tareas del mes y sus pedidos. Los 7 accesos del módulo Taller abren las pantallas de siempre, con todos los autos.
   Para que esas pantallas muestren solo lo de un mecánico habría que cambiar varios casos de uso del taller (Gestionar y Consultar órdenes, Tareas realizadas, Diagnósticos, etc.) y sus servicios. Además habría que agregar, en EA, el filtro "por mecánico" a cada uno.

2. **Asignar el diagnóstico a un mecánico.**
   En el modelo, el diagnóstico no tiene mecánico. Por eso la tarjeta y la lista "Mis autos esperando diagnóstico" muestran todos los autos del taller que esperan diagnóstico.
   Para que sean "míos" de verdad haría falta una columna nueva en Diagnostico y cambiar el caso de uso "Registrar diagnóstico".

3. **Bloqueo temporal del usuario después de 3 intentos fallidos.**
   Hoy, después de 3 intentos se cierra el programa, como estaba definido.
   Un bloqueo que dure un tiempo (o hasta que el administrador lo desbloquee) necesita una columna o tabla nueva, y cambiar el caso de uso Iniciar sesión.

4. **Texto "Plate" en Seguridad > Permisos (en inglés).**
   La etiqueta "Patente:" de esa pantalla se traduce como "Plate" (patente de auto), cuando ahí significa "permiso" (patente del Composite). Las dos usan la misma clave de idioma.
   Para corregirlo hay que cambiar el texto de la etiqueta, por ejemplo a "Permiso (patente):". No lo cambié porque no estaba en la lista de puntos.

5. **Modo oscuro completo en lo que dibuja Windows.**
   Estas partes siguen claras: la franja de las pestañas de Permisos, el selector de fechas y las casillas de verificación. Solo se podrían cambiar dibujando esos controles enteros a mano, y eso es riesgoso a esta altura.
   Las barras de desplazamiento sí pasaron a oscuro.
