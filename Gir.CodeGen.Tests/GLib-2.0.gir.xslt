<xsl:stylesheet
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:core="http://www.gtk.org/introspection/core/1.0"
    xmlns:c="http://www.gtk.org/introspection/c/1.0"
    xmlns:glib="http://www.gtk.org/introspection/glib/1.0"
    xmlns:clr="http://www.gtk.org/introspection/clr/1.0"
    version="1.0">

    <xsl:template match="@* | node()">
        <xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template match="core:namespace">
        <xsl:copy>
            <xsl:apply-templates select="@* | *"/>
            <core:primitive name="gpointer" clr:name="System.IntPtr" />
            <core:primitive name="gboolean" clr:name="System.Boolean" />
            <core:primitive name="gint8" clr:name="System.SByte" />
            <core:primitive name="gint16" clr:name="System.Int16" />
            <core:primitive name="gint32" clr:name="System.Int32" />
            <core:primitive name="gint64" clr:name="System.Int64" />
            <core:primitive name="guint8" clr:name="System.Byte" />
            <core:primitive name="guint16" clr:name="System.UInt16" />
            <core:primitive name="guint32" clr:name="System.UInt32" />
            <core:primitive name="guint64" clr:name="System.UInt64" />
            <core:primitive name="gint" clr:name="System.Int32" />
            <core:primitive name="guint" clr:name="System.UInt32" />
            <core:primitive name="glong" clr:name="System.Int64" />
            <core:primitive name="gulong" clr:name="System.UInt64" />
            <core:primitive name="gfloat" clr:name="System.Single" />
            <core:primitive name="gdouble" clr:name="System.Double" />
            <core:primitive name="gchar" clr:name="System.SByte" />
            <core:primitive name="guchar" clr:name="System.Byte" />
            <core:primitive name="gsize" clr:name="System.UInt64" />
            <core:primitive name="gssize" clr:name="System.Int64" />
            <core:primitive name="utf8" clr:name="System.String" />
            <core:primitive name="filename" clr:name="System.String" clr:marshaler="Gir.FilenameMarshaler, Gir" />

            <core:primitive name="GType" clr:name="GLib.Type" />
            <core:primitive name="gunichar" clr:name="GLib.Unichar" />
        </xsl:copy>
    </xsl:template>

</xsl:stylesheet>