namespace It270.MedicalSystem.Common.Application.Core.Contracts
{
    // Usar 'record' para inmutabilidad y concisión en mensajes
    public record EmailMessageWithAttachment // Ya no es 'I', porque es una implementación de datos
    (
        string Email,
        string Name,
        string Subject,
        string Content, // HTML content
        string? AttachmentUrl, // URL del adjunto en el storage
        string? AttachmentFileName, // Nombre original del adjunto
        string? AttachmentContentType // Tipo de contenido original del adjunto
    );
}