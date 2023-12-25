using System.Net;

namespace IdentityProvider.Dtos;


public class ApiResult
{
    public ApiResult(string message = "", HttpStatusCode code = HttpStatusCode.OK, bool success = true)
    {
        Status = code;
        Message = message;
        Success = success;
    }
    public HttpStatusCode Status { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
}
public class ApiResultHandler
{

    public static ApiResult Ok(string message = "", HttpStatusCode code = HttpStatusCode.OK) =>
                                                               new ApiResult(message, code, true);
    public static ApiResult Failed(string message = "", HttpStatusCode code = HttpStatusCode.BadRequest) =>
                                                               new ApiResult(message, code, false);


}
public class ApiResult<T> : ApiResult
{
    public ApiResult(T data, string message = "", HttpStatusCode code = HttpStatusCode.OK, bool success = true)
    {
        Data = data;
        Status = code;
        Message = message;
        Success = success;
    }
    public T Data { get; set; }
}

public class ApiResultHandler<T>
{

    public static ApiResult<T> Ok(T data, string message = "", HttpStatusCode code = HttpStatusCode.OK) =>
                                                               new ApiResult<T>(data, message, code, true);
    public static ApiResult<T> Failed(T data, string message = "", HttpStatusCode code = HttpStatusCode.BadRequest) =>
                                                               new ApiResult<T>(data, message, code, false);


}
public class RoleForDropDownDto
{
    public string Value { get; set; }
    public string Key { get; set; }
}
public class RoleDto
{
    public string Name { get; set; }
    public Guid Id { get; set; }
}
public class AddRoleDto
{
    public string Name { get; set; }
}
public class EditRoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
public class AddRolePermissionDto
{
    public Guid RoleId { get; set; }
    public int[] ActionId { get; set; }
}

public class PermissionListDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

