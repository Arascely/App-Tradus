namespace TradusApp.Domain.Entities;

public class LibroEtiqueta
{
    public Guid LibroId { get; set; }
    public Guid EtiquetaId { get; set; }

    public Libro? Libro { get; set; }
    public Etiqueta? Etiqueta { get; set; }
}
