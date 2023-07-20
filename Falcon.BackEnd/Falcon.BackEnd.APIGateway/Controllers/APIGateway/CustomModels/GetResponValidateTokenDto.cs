namespace Falcon.BackEnd.APIGateway.Controllers.APIGateway.CustomModels
{
    public class GetResponValidateTokenDto
    {
        public string objRequestData { get; set; } = string.Empty; 
        public bool bitSuccess { get; set; }
        public GetResponObjData? objData { get; set; }
        public bool? txtMessage { get; set; }
        public bool bitError { get; set; }
        public bool? txtErrorMessage { get; set; }
    }

    public class GetResponObjData
    { 
        public bool active { get; set; } 
    }
}
