namespace PRAS.Services.DomainModel
{
    /// <summary>
    /// Resultado de verificar UNA tabla protegida con dígitos verificadores.
    /// Lo muestra la pantalla "Integridad de datos" para que se vea qué se revisó:
    /// cuántos registros tiene la tabla, cuántos fueron modificados por fuera (DVH)
    /// y si se agregaron o borraron filas (DVV).
    /// </summary>
    public class ResultadoIntegridadTabla
    {
        /// <summary>Nombre de la tabla. Ej: "Usuario".</summary>
        public string Tabla { get; set; }

        /// <summary>Base donde está la tabla. Ej: "PRAS_Seguridad".</summary>
        public string Base { get; set; }

        /// <summary>Cantidad de registros revisados uno por uno (DVH).</summary>
        public int Registros { get; set; }

        /// <summary>Registros cuyo DVH no coincide (alguien cambió un dato con SQL).</summary>
        public int RegistrosAlterados { get; set; }

        /// <summary>false si se agregaron o borraron filas por fuera del sistema (DVV).</summary>
        public bool DvvCorrecto { get; set; }

        /// <summary>true si la tabla está bien: ningún DVH alterado y el DVV coincide.</summary>
        public bool Integra => RegistrosAlterados == 0 && DvvCorrecto;
    }
}
