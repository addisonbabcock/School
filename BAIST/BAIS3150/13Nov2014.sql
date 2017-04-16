

select ShipperID, CompanyName, Phone from shippers
for xml auto, xmlschema

/*
raw - generates a single 'row' element per row in the resulting rowset

<row ShipperID="1" CompanyName="Speedy Express" Phone="(503) 555-9831" />
<row ShipperID="2" CompanyName="United Package" Phone="(503) 555-3199" />
<row ShipperID="3" CompanyName="Federal Shipping" Phone="(503) 555-9931" />

auto - generates a single 'xxx' element per row in the resulting rowset
Can be the same as FOR XML RAW('xxx')

<shippers ShipperID="1" CompanyName="Speedy Express" Phone="(503) 555-9831" />
<shippers ShipperID="2" CompanyName="United Package" Phone="(503) 555-3199" />
<shippers ShipperID="3" CompanyName="Federal Shipping" Phone="(503) 555-9931" />

auto, elements - Breaks out the attributes into nested elements

<shippers>
  <ShipperID>1</ShipperID>
  <CompanyName>Speedy Express</CompanyName>
  <Phone>(503) 555-9831</Phone>
</shippers>
<shippers>
  <ShipperID>2</ShipperID>
  <CompanyName>United Package</CompanyName>
  <Phone>(503) 555-3199</Phone>
</shippers>
<shippers>
  <ShipperID>3</ShipperID>
  <CompanyName>Federal Shipping</CompanyName>
  <Phone>(503) 555-9931</Phone>
</shippers>

auto, xmldata - generates a XDR schema inline before the data

<Schema name="Schema1" xmlns="urn:schemas-microsoft-com:xml-data" xmlns:dt="urn:schemas-microsoft-com:datatypes">
  <ElementType name="shippers" content="empty" model="closed">
    <AttributeType name="ShipperID" dt:type="i4" />
    <AttributeType name="CompanyName" dt:type="string" />
    <AttributeType name="Phone" dt:type="string" />
    <attribute type="ShipperID" />
    <attribute type="CompanyName" />
    <attribute type="Phone" />
  </ElementType>
</Schema>
<shippers xmlns="x-schema:#Schema1" ShipperID="1" CompanyName="Speedy Express" Phone="(503) 555-9831" />
<shippers xmlns="x-schema:#Schema1" ShipperID="2" CompanyName="United Package" Phone="(503) 555-3199" />
<shippers xmlns="x-schema:#Schema1" ShipperID="3" CompanyName="Federal Shipping" Phone="(503) 555-9931" />

auto, xmlschema - generates an xml xsd schema inline before the data

<xsd:schema targetNamespace="urn:schemas-microsoft-com:sql:SqlRowSet1" xmlns:schema="urn:schemas-microsoft-com:sql:SqlRowSet1" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:sqltypes="http://schemas.microsoft.com/sqlserver/2004/sqltypes" elementFormDefault="qualified">
  <xsd:import namespace="http://schemas.microsoft.com/sqlserver/2004/sqltypes" schemaLocation="http://schemas.microsoft.com/sqlserver/2004/sqltypes/sqltypes.xsd" />
  <xsd:element name="shippers">
    <xsd:complexType>
      <xsd:attribute name="ShipperID" type="sqltypes:int" use="required" />
      <xsd:attribute name="CompanyName" use="required">
        <xsd:simpleType>
          <xsd:restriction base="sqltypes:nvarchar" sqltypes:localeId="1033" sqltypes:sqlCompareOptions="IgnoreCase IgnoreKanaType IgnoreWidth" sqltypes:sqlSortId="52">
            <xsd:maxLength value="40" />
          </xsd:restriction>
        </xsd:simpleType>
      </xsd:attribute>
      <xsd:attribute name="Phone">
        <xsd:simpleType>
          <xsd:restriction base="sqltypes:nvarchar" sqltypes:localeId="1033" sqltypes:sqlCompareOptions="IgnoreCase IgnoreKanaType IgnoreWidth" sqltypes:sqlSortId="52">
            <xsd:maxLength value="24" />
          </xsd:restriction>
        </xsd:simpleType>
      </xsd:attribute>
    </xsd:complexType>
  </xsd:element>
</xsd:schema>
<shippers xmlns="urn:schemas-microsoft-com:sql:SqlRowSet1" ShipperID="1" CompanyName="Speedy Express" Phone="(503) 555-9831" />
<shippers xmlns="urn:schemas-microsoft-com:sql:SqlRowSet1" ShipperID="2" CompanyName="United Package" Phone="(503) 555-3199" />
<shippers xmlns="urn:schemas-microsoft-com:sql:SqlRowSet1" ShipperID="3" CompanyName="Federal Shipping" Phone="(503) 555-9931" />

root('xxx') - gives the results a root element with the name 'xxx'

<xxx>
	<shippers ShipperID="1" CompanyName="Speedy Express" Phone="(503) 555-9831" />
	<shippers ShipperID="2" CompanyName="United Package" Phone="(503) 555-3199" />
	<shippers ShipperID="3" CompanyName="Federal Shipping" Phone="(503) 555-9931" />
</xxx>

explicit - 



path - 


*/