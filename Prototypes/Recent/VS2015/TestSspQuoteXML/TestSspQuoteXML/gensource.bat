xsd XmlDocuments\AGG09102017.one.xml /o:XmlDocuments
xsd /c /n:TestSspQuoteXML XmlDocuments\AGG09102017.one.xsd
@if exist EISExtracts.cs del EISExtracts.cs
ren AGG09102017_one.cs EISExtracts.cs
