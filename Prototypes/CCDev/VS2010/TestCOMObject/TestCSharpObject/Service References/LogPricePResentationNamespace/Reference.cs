﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestCSharpObject.LogPricePResentationNamespace {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://services.endsleigh.co.uk/hostservices/LogPricePresentation", ConfigurationName="LogPricePResentationNamespace.LogPricePresentationPortType")]
    public interface LogPricePresentationPortType {
        
        // CODEGEN: Generating message contract since the operation LogPricePresentationOperation is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        TestCSharpObject.LogPricePResentationNamespace.LogPricePresentationOperationResponse LogPricePresentationOperation(TestCSharpObject.LogPricePResentationNamespace.LogPricePresentationOperationRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://services.endsleigh.co.uk/hostservices/LogPricePresentation/schema")]
    public partial class LogPricePresentationRequest : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string schemeCodeField;
        
        private string channelCodeField;
        
        private string agencyCodeField;
        
        private string techRefField;
        
        private string machineField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string SchemeCode {
            get {
                return this.schemeCodeField;
            }
            set {
                this.schemeCodeField = value;
                this.RaisePropertyChanged("SchemeCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ChannelCode {
            get {
                return this.channelCodeField;
            }
            set {
                this.channelCodeField = value;
                this.RaisePropertyChanged("ChannelCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string AgencyCode {
            get {
                return this.agencyCodeField;
            }
            set {
                this.agencyCodeField = value;
                this.RaisePropertyChanged("AgencyCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string TechRef {
            get {
                return this.techRefField;
            }
            set {
                this.techRefField = value;
                this.RaisePropertyChanged("TechRef");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string Machine {
            get {
                return this.machineField;
            }
            set {
                this.machineField = value;
                this.RaisePropertyChanged("Machine");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://services.endsleigh.co.uk/hostservices/LogPricePresentation/schema")]
    public partial class LogPricePresentationResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string referenceField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer", Order=0)]
        public string Reference {
            get {
                return this.referenceField;
            }
            set {
                this.referenceField = value;
                this.RaisePropertyChanged("Reference");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class LogPricePresentationOperationRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://services.endsleigh.co.uk/hostservices/LogPricePresentation/schema", Order=0)]
        public TestCSharpObject.LogPricePResentationNamespace.LogPricePresentationRequest LogPricePresentationRequest;
        
        public LogPricePresentationOperationRequest() {
        }
        
        public LogPricePresentationOperationRequest(TestCSharpObject.LogPricePResentationNamespace.LogPricePresentationRequest LogPricePresentationRequest) {
            this.LogPricePresentationRequest = LogPricePresentationRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class LogPricePresentationOperationResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://services.endsleigh.co.uk/hostservices/LogPricePresentation/schema", Order=0)]
        public TestCSharpObject.LogPricePResentationNamespace.LogPricePresentationResponse LogPricePresentationResponse;
        
        public LogPricePresentationOperationResponse() {
        }
        
        public LogPricePresentationOperationResponse(TestCSharpObject.LogPricePResentationNamespace.LogPricePresentationResponse LogPricePresentationResponse) {
            this.LogPricePresentationResponse = LogPricePresentationResponse;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface LogPricePresentationPortTypeChannel : TestCSharpObject.LogPricePResentationNamespace.LogPricePresentationPortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LogPricePresentationPortTypeClient : System.ServiceModel.ClientBase<TestCSharpObject.LogPricePResentationNamespace.LogPricePresentationPortType>, TestCSharpObject.LogPricePResentationNamespace.LogPricePresentationPortType {
        
        public LogPricePresentationPortTypeClient() {
        }
        
        public LogPricePresentationPortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LogPricePresentationPortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LogPricePresentationPortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LogPricePresentationPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        TestCSharpObject.LogPricePResentationNamespace.LogPricePresentationOperationResponse TestCSharpObject.LogPricePResentationNamespace.LogPricePresentationPortType.LogPricePresentationOperation(TestCSharpObject.LogPricePResentationNamespace.LogPricePresentationOperationRequest request) {
            return base.Channel.LogPricePresentationOperation(request);
        }
        
        public TestCSharpObject.LogPricePResentationNamespace.LogPricePresentationResponse LogPricePresentationOperation(TestCSharpObject.LogPricePResentationNamespace.LogPricePresentationRequest LogPricePresentationRequest) {
            TestCSharpObject.LogPricePResentationNamespace.LogPricePresentationOperationRequest inValue = new TestCSharpObject.LogPricePResentationNamespace.LogPricePresentationOperationRequest();
            inValue.LogPricePresentationRequest = LogPricePresentationRequest;
            TestCSharpObject.LogPricePResentationNamespace.LogPricePresentationOperationResponse retVal = ((TestCSharpObject.LogPricePResentationNamespace.LogPricePresentationPortType)(this)).LogPricePresentationOperation(inValue);
            return retVal.LogPricePresentationResponse;
        }
    }
}
