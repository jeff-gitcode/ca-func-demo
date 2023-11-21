using Application.Abstraction;
using FluentValidation;
using Microsoft.Azure.Functions.Worker.Http;

namespace CleanFunctionApp.Function;

public static class RequestConverter
{
    public static async Task<T> ValidateAndConvert<T>(this HttpRequestData req) where T : class
    {
        var dto = await req.ReadFromJsonAsync<T>();
        // Validate with FluentValidation
        if (dto is IValidateable<T>) (dto as IValidateable<T>)!.Validator.ValidateAndThrow(dto);
        return dto!;
    } 
}