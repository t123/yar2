<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" omit-xml-declaration="yes" indent="yes" />
  <xsl:template match="/root">
    <xsl:apply-templates select="content"/>
  </xsl:template>
  <xsl:template match="content">
    <table>
      <xsl:choose>
        <xsl:when test="/root/content/@isParallel='true'">
          <xsl:attribute name="class">
            <xsl:text>reading-table parallel</xsl:text>
          </xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="class">
            <xsl:text>reading-table single</xsl:text>
          </xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
      
      <tr class="header">
        <xsl:choose>
          <xsl:when test="/root/content/@isParallel='true'">
            <td colspan="2">
              <!--BLANK-->
            </td>
          </xsl:when>
          <xsl:otherwise>
            <td>
              <!--BLANK-->
            </td>
          </xsl:otherwise>
        </xsl:choose>
      </tr>

      <xsl:apply-templates select="join"/>
      <tr class="footer">
        <xsl:choose>
          <xsl:when test="/root/content/@isParallel='true'">
            <td colspan="2">
              <!--BLANK-->
            </td>
          </xsl:when>
          <xsl:otherwise>
            <td>
              <!--BLANK-->
            </td>
          </xsl:otherwise>
        </xsl:choose>
      </tr>
    </table>
  </xsl:template>
  <xsl:template match="join">
    <tr>
      <xsl:apply-templates select="paragraph" />
      <xsl:choose>
        <xsl:when test="/root/content/@isParallel='true'">
          <td class="disabled">
            <xsl:attribute name="dir">
              <xsl:value-of select="translation/@direction"/>
            </xsl:attribute>
            <xsl:value-of select="translation"/>
          </td>
        </xsl:when>
      </xsl:choose>
    </tr>
  </xsl:template>
  <xsl:template match="paragraph">
    <td class="__active">
      <xsl:attribute name="dir">
        <xsl:value-of select="@direction"/>
      </xsl:attribute>
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
      <xsl:attribute name="data-definition">
        <xsl:value-of select="@definition"/>
      </xsl:attribute>
      <xsl:apply-templates />
    </span>
  </xsl:template>
  <xsl:template match="term">
    <span>
      <xsl:attribute name="class">
        <xsl:text>__term</xsl:text>
        <xsl:text> --</xsl:text>
        <xsl:value-of select="@phraseClass"/>
        <xsl:if test="@frequency-commonness > 0">
          <xsl:text> __common</xsl:text>
        </xsl:if>
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
            <xsl:if test="string-length(@definition)>0 and @state='unknown'">
              <xsl:text> __ud</xsl:text>
            </xsl:if>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="data-lower">
        <xsl:value-of select="@phrase"/>
      </xsl:attribute>
      <xsl:attribute name="data-definition">
        <xsl:value-of select="@definition" disable-output-escaping="yes"/>
      </xsl:attribute>
      <xsl:attribute name="data-occurrences">
        <xsl:value-of select="@occurrences" disable-output-escaping="yes"/>
      </xsl:attribute>
      <xsl:attribute name="data-frequency-total">
        <xsl:value-of select="@frequency-total" disable-output-escaping="yes"/>
      </xsl:attribute>
      <xsl:attribute name="data-frequency-notseen">
        <xsl:value-of select="@frequency-notseen" disable-output-escaping="yes"/>
      </xsl:attribute>
      <xsl:attribute name="data-frequency-commonness">
        <xsl:value-of select="@frequency-commonness" disable-output-escaping="yes"/>
      </xsl:attribute>     
      <xsl:value-of select="."/>
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