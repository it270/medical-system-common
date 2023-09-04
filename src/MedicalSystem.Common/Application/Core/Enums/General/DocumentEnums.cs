namespace It270.MedicalSystem.Common.Application.Core.Enums;

/// <summary>
/// Document enums
/// </summary>
public class DocumentEnums
{
    /// <summary>
    /// Document type enum (patients)
    /// </summary>
    public enum PatientsDocumentTypeEnum : int
    {
        /// <summary>
        /// Número de identificación tributaria
        /// </summary>
        NIT = 1,

        /// <summary>
        /// Cédula de ciudadanía
        /// </summary>
        CC,

        /// <summary>
        /// Cédula de extranjería
        /// </summary>
        CE,

        /// <summary>
        /// Tarjeta de identidad
        /// </summary>
        TI,

        /// <summary>
        /// Passport
        /// </summary>
        PA,

        /// <summary>
        /// Registro civil
        /// </summary>
        RC,

        /// <summary>
        /// Adulto sin identificar
        /// </summary>
        AS,

        /// <summary>
        /// Menor sin identificar
        /// </summary>
        MS,

        /// <summary>
        /// Permiso especial de permanencia
        /// </summary>
        PE,

        /// <summary>
        /// Documento extranjero
        /// </summary>
        DE,

        /// <summary>
        /// Salvo conducto de permanencia
        /// </summary>
        SC,

        /// <summary>
        /// Certificado nacido vivo
        /// </summary>
        CN,

        /// <summary>
        /// Carné diplomático
        /// </summary>
        CD,
    }
}