﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Asiservy.Automatizacion.Formularios.DataLifeService {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DataLifeService.ServicioAsiservySoap")]
    public interface ServicioAsiservySoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/actualizarDatosEmpleados", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet actualizarDatosEmpleados(string cedula, string compania, string direccion, string barrio, string telefono, string numsoc, string mailjefe);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/actualizarDatosEmpleados", ReplyAction="*")]
        System.Threading.Tasks.Task<System.Data.DataSet> actualizarDatosEmpleadosAsync(string cedula, string compania, string direccion, string barrio, string telefono, string numsoc, string mailjefe);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/actualizarCodigosEmpleados", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet actualizarCodigosEmpleados(string cedula, string compania, string codigoDepartamento, string codigoCargo, string codigoSubarea, string codigoArea);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/actualizarCodigosEmpleados", ReplyAction="*")]
        System.Threading.Tasks.Task<System.Data.DataSet> actualizarCodigosEmpleadosAsync(string cedula, string compania, string codigoDepartamento, string codigoCargo, string codigoSubarea, string codigoArea);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ServicioAsiservySoapChannel : Asiservy.Automatizacion.Formularios.DataLifeService.ServicioAsiservySoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicioAsiservySoapClient : System.ServiceModel.ClientBase<Asiservy.Automatizacion.Formularios.DataLifeService.ServicioAsiservySoap>, Asiservy.Automatizacion.Formularios.DataLifeService.ServicioAsiservySoap {
        
        public ServicioAsiservySoapClient() {
        }
        
        public ServicioAsiservySoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServicioAsiservySoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioAsiservySoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioAsiservySoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Data.DataSet actualizarDatosEmpleados(string cedula, string compania, string direccion, string barrio, string telefono, string numsoc, string mailjefe) {
            return base.Channel.actualizarDatosEmpleados(cedula, compania, direccion, barrio, telefono, numsoc, mailjefe);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> actualizarDatosEmpleadosAsync(string cedula, string compania, string direccion, string barrio, string telefono, string numsoc, string mailjefe) {
            return base.Channel.actualizarDatosEmpleadosAsync(cedula, compania, direccion, barrio, telefono, numsoc, mailjefe);
        }
        
        public System.Data.DataSet actualizarCodigosEmpleados(string cedula, string compania, string codigoDepartamento, string codigoCargo, string codigoSubarea, string codigoArea) {
            return base.Channel.actualizarCodigosEmpleados(cedula, compania, codigoDepartamento, codigoCargo, codigoSubarea, codigoArea);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> actualizarCodigosEmpleadosAsync(string cedula, string compania, string codigoDepartamento, string codigoCargo, string codigoSubarea, string codigoArea) {
            return base.Channel.actualizarCodigosEmpleadosAsync(cedula, compania, codigoDepartamento, codigoCargo, codigoSubarea, codigoArea);
        }
    }
}
