<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" omit-xml-declaration="yes" indent="yes" />
  <xsl:template match="/root">
    <xsl:apply-templates select="content"/>
  </xsl:template>
  <xsl:template match="content">
    <table>
      <xsl:apply-templates select="join"/>
    </table>
  </xsl:template>
  <xsl:template match="join">
    <tr>
      <xsl:apply-templates select="paragraph" />
      <xsl:choose>
        <xsl:when test="/root/content/@isParallel='true'">
          <td class="disabled" width="50%">
            <xsl:value-of select="translation"/>
          </td>
        </xsl:when>
      </xsl:choose>
    </tr>
  </xsl:template>
  <xsl:template match="paragraph">
    <td class="active">
      <xsl:attribute name="dir">
        <xsl:value-of select="@direction"/>
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="/root/content/@isParallel='true'">
          <xsl:attribute name="width">49%</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="width">
            100%
          </xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:apply-templates />
    </td>
  </xsl:template>
  <xsl:template match="fragment">
    <span>
      <xsl:attribute name="class">
        <xsl:text>__fragment</xsl:text>
        <xsl:text> __</xsl:text>
        <xsl:value-of select="@state"/>
        <xsl:text> __</xsl:text>
        <xsl:value-of select="@termId"/>
      </xsl:attribute>
      <xsl:attribute name="data-termid">
        <xsl:value-of select="@termId"/>
      </xsl:attribute>
      <xsl:apply-templates />
    </span>
  </xsl:template>
  <xsl:template match="term">
    <span>
      <xsl:attribute name="class">
        <xsl:text>__term</xsl:text>
        <xsl:text> __</xsl:text>
        <xsl:value-of select="@phraseClass"/>
        <xsl:choose>
          <xsl:when test="ancestor::fragment">
            <xsl:text> __</xsl:text>
            <xsl:value-of select="@state"/>
            <xsl:text>_t</xsl:text>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text> __</xsl:text>
            <xsl:value-of select="@state"/>
            <xsl:if test="string-length(@definition)>0 and @state='known'">
              <xsl:text> __kd</xsl:text>
            </xsl:if>
            <xsl:if test="string-length(@definition)>0 and @state='ignored'">
              <xsl:text> __id</xsl:text>
            </xsl:if>
            <xsl:if test="string-length(@definition)>0 and @state='notknown'">
              <xsl:text> __ud</xsl:text>
            </xsl:if>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="data-lower">
        <xsl:value-of select="@phrase"/>
      </xsl:attribute>
      <xsl:value-of select="."/>
      <!--<xsl:choose>
        <xsl:when test="not(ancestor::fragment) and @definition">
          <a rel="tooltip">
            <xsl:attribute name="title">
              <xsl:value-of select="@definition" disable-output-escaping="yes"/>
            </xsl:attribute>
            <xsl:value-of select="."/>
          </a>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="."/>
        </xsl:otherwise>
      </xsl:choose>-->
      <xsl:if test="not(ancestor::fragment) and @definition">
        <span class="__tooltiptext">
          <xsl:value-of select="@definition"/>
        </span>
      </xsl:if>
    </span>
  </xsl:template>
  <xsl:template match="whitespace">
    <span class="__nt __whitespace">
      <xsl:value-of select="."/>
    </span>
  </xsl:template>
  <xsl:template match="number">
    <span class="__nt __number">
      <xsl:value-of select="."/>
    </span>
  </xsl:template>
  <xsl:template match="punctuation">
    <span class="__nt __punctuation">
      <xsl:value-of select="."/>
    </span>
  </xsl:template>
  <xsl:template match="tag">
    <xsl:value-of select="." disable-output-escaping="yes" />
  </xsl:template>
</xsl:stylesheet>