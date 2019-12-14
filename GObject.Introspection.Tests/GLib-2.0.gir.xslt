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
            <xsl:attribute name="clr:shared-library">glib</xsl:attribute>
            <xsl:apply-templates select="@* | *"/>
            <core:primitive name="gpointer" clr:type="System.IntPtr" />
            <core:primitive name="gboolean" clr:type="System.Boolean" />
            <core:primitive name="gint8" clr:type="System.SByte" />
            <core:primitive name="gint16" clr:type="System.Int16" />
            <core:primitive name="gint32" clr:type="System.Int32" />
            <core:primitive name="gint64" clr:type="System.Int64" />
            <core:primitive name="guint8" clr:type="System.Byte" />
            <core:primitive name="guint16" clr:type="System.UInt16" />
            <core:primitive name="guint32" clr:type="System.UInt32" />
            <core:primitive name="guint64" clr:type="System.UInt64" />
            <core:primitive name="gushort" clr:type="System.UInt16" />
            <core:primitive name="gint" clr:type="System.Int32" />
            <core:primitive name="guint" clr:type="System.UInt32" />
            <core:primitive name="glong" clr:type="System.Int64" />
            <core:primitive name="gulong" clr:type="System.UInt64" />
            <core:primitive name="gfloat" clr:type="System.Single" />
            <core:primitive name="gdouble" clr:type="System.Double" />
            <core:primitive name="gchar" clr:type="System.SByte" />
            <core:primitive name="guchar" clr:type="System.Byte" />
            <core:primitive name="gsize" clr:type="System.UInt64" />
            <core:primitive name="gssize" clr:type="System.Int64" />
            <core:primitive name="utf8" clr:type="System.String" clr:marshaler-type="GLib.Utf8Marshaler" />
            <core:primitive name="filename" clr:type="System.String" clr:marshaler-type="GLib.FilenameMarshaler" />
            <core:primitive name="gunichar" clr:type="GLib.Unichar" />

            <core:primitive name="GType" clr:type="GLib.GType" clr:marshaler-type="GLib.GTypeMarshaler" />
        </xsl:copy>
    </xsl:template>

    <xsl:template match="core:alias[@name='Type']/core:type">
        
    </xsl:template>

    <xsl:template match="core:record[@name='Variant']">
        <xsl:copy>
            <xsl:attribute name="clr:type">GLib.Variant</xsl:attribute>
            <xsl:attribute name="clr:marshaler-type">GLib.VariantMarshaler</xsl:attribute>
            <xsl:apply-templates select="@* | *" />
        </xsl:copy>
    </xsl:template>

    <xsl:template match="core:record[@name='TestLogMsg']/core:field[@name='nums']/core:type">
        <core:type name="gdouble" c:type="gdouble*" />
    </xsl:template>

    <xsl:template match="core:enumeration[@name='SpawnError']/core:member[@name='2big']">
        <!-- remove -->
    </xsl:template>

</xsl:stylesheet>
