﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Asiservy.Automatizacion.Formularios.OnlyControlService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="OnlyControlService.wsrvTcontrolSoap")]
    public interface wsrvTcontrolSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/verificarConexion", ReplyAction="*")]
        bool verificarConexion();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/verificarConexion", ReplyAction="*")]
        System.Threading.Tasks.Task<bool> verificarConexionAsync();
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento empeID del espacio de nombres http://tempuri.org/ no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/insertarPermiso", ReplyAction="*")]
        Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoResponse insertarPermiso(Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/insertarPermiso", ReplyAction="*")]
        System.Threading.Tasks.Task<Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoResponse> insertarPermisoAsync(Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoRequest request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento key del espacio de nombres http://tempuri.org/ no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/consultarCatalogoPermisos", ReplyAction="*")]
        Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosResponse consultarCatalogoPermisos(Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/consultarCatalogoPermisos", ReplyAction="*")]
        System.Threading.Tasks.Task<Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosResponse> consultarCatalogoPermisosAsync(Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class insertarPermisoRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="insertarPermiso", Namespace="http://tempuri.org/", Order=0)]
        public Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoRequestBody Body;
        
        public insertarPermisoRequest() {
        }
        
        public insertarPermisoRequest(Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class insertarPermisoRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public System.DateTime fini;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public System.DateTime ffin;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string empeID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string tipoP;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=4)]
        public bool fDia;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=5)]
        public System.DateTime hini;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=6)]
        public System.DateTime hfin;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=7)]
        public int turno;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=8)]
        public string obs;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=9)]
        public string key;
        
        public insertarPermisoRequestBody() {
        }
        
        public insertarPermisoRequestBody(System.DateTime fini, System.DateTime ffin, string empeID, string tipoP, bool fDia, System.DateTime hini, System.DateTime hfin, int turno, string obs, string key) {
            this.fini = fini;
            this.ffin = ffin;
            this.empeID = empeID;
            this.tipoP = tipoP;
            this.fDia = fDia;
            this.hini = hini;
            this.hfin = hfin;
            this.turno = turno;
            this.obs = obs;
            this.key = key;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class insertarPermisoResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="insertarPermisoResponse", Namespace="http://tempuri.org/", Order=0)]
        public Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoResponseBody Body;
        
        public insertarPermisoResponse() {
        }
        
        public insertarPermisoResponse(Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class insertarPermisoResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string insertarPermisoResult;
        
        public insertarPermisoResponseBody() {
        }
        
        public insertarPermisoResponseBody(string insertarPermisoResult) {
            this.insertarPermisoResult = insertarPermisoResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class consultarCatalogoPermisosRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="consultarCatalogoPermisos", Namespace="http://tempuri.org/", Order=0)]
        public Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosRequestBody Body;
        
        public consultarCatalogoPermisosRequest() {
        }
        
        public consultarCatalogoPermisosRequest(Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class consultarCatalogoPermisosRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string key;
        
        public consultarCatalogoPermisosRequestBody() {
        }
        
        public consultarCatalogoPermisosRequestBody(string key) {
            this.key = key;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class consultarCatalogoPermisosResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="consultarCatalogoPermisosResponse", Namespace="http://tempuri.org/", Order=0)]
        public Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosResponseBody Body;
        
        public consultarCatalogoPermisosResponse() {
        }
        
        public consultarCatalogoPermisosResponse(Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class consultarCatalogoPermisosResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string consultarCatalogoPermisosResult;
        
        public consultarCatalogoPermisosResponseBody() {
        }
        
        public consultarCatalogoPermisosResponseBody(string consultarCatalogoPermisosResult) {
            this.consultarCatalogoPermisosResult = consultarCatalogoPermisosResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface wsrvTcontrolSoapChannel : Asiservy.Automatizacion.Formularios.OnlyControlService.wsrvTcontrolSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class wsrvTcontrolSoapClient : System.ServiceModel.ClientBase<Asiservy.Automatizacion.Formularios.OnlyControlService.wsrvTcontrolSoap>, Asiservy.Automatizacion.Formularios.OnlyControlService.wsrvTcontrolSoap {
        
        public wsrvTcontrolSoapClient() {
        }
        
        public wsrvTcontrolSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public wsrvTcontrolSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public wsrvTcontrolSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public wsrvTcontrolSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool verificarConexion() {
            return base.Channel.verificarConexion();
        }
        
        public System.Threading.Tasks.Task<bool> verificarConexionAsync() {
            return base.Channel.verificarConexionAsync();
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoResponse Asiservy.Automatizacion.Formularios.OnlyControlService.wsrvTcontrolSoap.insertarPermiso(Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoRequest request) {
            return base.Channel.insertarPermiso(request);
        }
        
        public string insertarPermiso(System.DateTime fini, System.DateTime ffin, string empeID, string tipoP, bool fDia, System.DateTime hini, System.DateTime hfin, int turno, string obs, string key) {
            Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoRequest inValue = new Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoRequest();
            inValue.Body = new Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoRequestBody();
            inValue.Body.fini = fini;
            inValue.Body.ffin = ffin;
            inValue.Body.empeID = empeID;
            inValue.Body.tipoP = tipoP;
            inValue.Body.fDia = fDia;
            inValue.Body.hini = hini;
            inValue.Body.hfin = hfin;
            inValue.Body.turno = turno;
            inValue.Body.obs = obs;
            inValue.Body.key = key;
            Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoResponse retVal = ((Asiservy.Automatizacion.Formularios.OnlyControlService.wsrvTcontrolSoap)(this)).insertarPermiso(inValue);
            return retVal.Body.insertarPermisoResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoResponse> Asiservy.Automatizacion.Formularios.OnlyControlService.wsrvTcontrolSoap.insertarPermisoAsync(Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoRequest request) {
            return base.Channel.insertarPermisoAsync(request);
        }
        
        public System.Threading.Tasks.Task<Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoResponse> insertarPermisoAsync(System.DateTime fini, System.DateTime ffin, string empeID, string tipoP, bool fDia, System.DateTime hini, System.DateTime hfin, int turno, string obs, string key) {
            Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoRequest inValue = new Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoRequest();
            inValue.Body = new Asiservy.Automatizacion.Formularios.OnlyControlService.insertarPermisoRequestBody();
            inValue.Body.fini = fini;
            inValue.Body.ffin = ffin;
            inValue.Body.empeID = empeID;
            inValue.Body.tipoP = tipoP;
            inValue.Body.fDia = fDia;
            inValue.Body.hini = hini;
            inValue.Body.hfin = hfin;
            inValue.Body.turno = turno;
            inValue.Body.obs = obs;
            inValue.Body.key = key;
            return ((Asiservy.Automatizacion.Formularios.OnlyControlService.wsrvTcontrolSoap)(this)).insertarPermisoAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosResponse Asiservy.Automatizacion.Formularios.OnlyControlService.wsrvTcontrolSoap.consultarCatalogoPermisos(Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosRequest request) {
            return base.Channel.consultarCatalogoPermisos(request);
        }
        
        public string consultarCatalogoPermisos(string key) {
            Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosRequest inValue = new Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosRequest();
            inValue.Body = new Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosRequestBody();
            inValue.Body.key = key;
            Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosResponse retVal = ((Asiservy.Automatizacion.Formularios.OnlyControlService.wsrvTcontrolSoap)(this)).consultarCatalogoPermisos(inValue);
            return retVal.Body.consultarCatalogoPermisosResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosResponse> Asiservy.Automatizacion.Formularios.OnlyControlService.wsrvTcontrolSoap.consultarCatalogoPermisosAsync(Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosRequest request) {
            return base.Channel.consultarCatalogoPermisosAsync(request);
        }
        
        public System.Threading.Tasks.Task<Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosResponse> consultarCatalogoPermisosAsync(string key) {
            Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosRequest inValue = new Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosRequest();
            inValue.Body = new Asiservy.Automatizacion.Formularios.OnlyControlService.consultarCatalogoPermisosRequestBody();
            inValue.Body.key = key;
            return ((Asiservy.Automatizacion.Formularios.OnlyControlService.wsrvTcontrolSoap)(this)).consultarCatalogoPermisosAsync(inValue);
        }
    }
}
