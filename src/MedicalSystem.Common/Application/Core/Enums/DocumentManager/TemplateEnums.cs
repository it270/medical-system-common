namespace It270.MedicalSystem.Common.Application.Core.Enums.DocumentManager;

/// <summary>
/// Template enums (for document manager)
/// </summary>
public static class TemplateEnums
{
    /// <summary>
    /// Template section enum
    /// </summary>
    public enum TemplateSectionEnum : int
    {
        /// <summary>
        /// HTML header section
        /// </summary>
        Header,

        /// <summary>
        /// HTML main section
        /// </summary>
        Main,

        /// <summary>
        /// HTML footer section
        /// </summary>
        Footer
    }

    /// <summary>
    /// Param type enum
    /// </summary>
    public enum TemplateParamTypeEnum : int
    {
        /// <summary>
        /// Text param
        /// </summary>
        Text = 1,

        /// <summary>
        /// Boolean param
        /// </summary>
        Bool,

        /// <summary>
        /// Integer param
        /// </summary>
        Int,

        /// <summary>
        /// Decimal param
        /// </summary>
        Decimal,

        /// <summary>
        /// Date param
        /// </summary>
        Date,

        /// <summary>
        /// Date time param
        /// </summary>
        DateTime,

        /// <summary>
        /// Time param
        /// </summary>
        Time,

        /// <summary>
        /// Image url param
        /// </summary>
        Image,

        /// <summary>
        /// List object param
        /// </summary>
        List,

        /// <summary>
        /// Table object param
        /// </summary>
        Table,

        /// <summary>
        /// Table row object param
        /// </summary>
        Row,
    }
}