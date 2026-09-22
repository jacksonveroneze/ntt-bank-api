namespace NttBankApi.Application.Abstractions.UseCases;

public interface IBaseRequest;

public interface IUseCase<in TRequest, TResponse>
    where TRequest : IBaseRequest
{
    Task<TResponse> ExecuteAsync(
        TRequest request,
        CancellationToken cancellationToken);
}
