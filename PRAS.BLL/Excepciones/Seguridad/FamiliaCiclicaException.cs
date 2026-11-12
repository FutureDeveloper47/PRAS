namespace PRAS.BLL.Excepciones
{
    /// <summary>Se quiso meter una familia dentro de sí misma (directa o indirectamente). Eso armaría un círculo infinito al recorrer el árbol.</summary>
    public class FamiliaCiclicaException : PermisoBusinessException
    {
        public FamiliaCiclicaException(string hija, string padre)
            : base($"No se puede agregar la familia \"{hija}\" dentro de \"{padre}\": \"{padre}\" ya forma parte de \"{hija}\".", "FAMILIA_CICLICA")
        {
        }
    }
}
