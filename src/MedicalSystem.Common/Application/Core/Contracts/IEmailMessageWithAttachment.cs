namespace It270.MedicalSystem.Common.Application.Core.Contracts
{
    /// <summary>
    /// Message contract for emails with attachments.
    /// </summary>
    public interface IEmailMessageWithAttachment
    {
        // Propiedades consistentes con GeneralMessage
        string Name { get; } // Nombre del remitente o contexto (ej: "comite")
        string Email { get; } // Correo electrónico del destinatario
        string Subject { get; }
        string Content { get; } // Contenido HTML del correo

        // Propiedades opcionales para el adjunto
        string? AttachmentUrl { get; } // URL o ruta donde se guardó el archivo
        string? AttachmentFileName { get; } // Nombre original del archivo (ej: "documento.pdf")
        string? AttachmentContentType { get; } // Tipo MIME (ej: "application/pdf")
        long? AttachmentFileSize { get; } // Tamaño en bytes
    }
}