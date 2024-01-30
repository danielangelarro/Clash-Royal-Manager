using ClashRoyalManager.Application.Common.Interfaces;
using ClashRoyalManager.Application.Common.Interfaces.Services;
using ClashRoyalManager.Application.Entities;
using ClashRoyalManager.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class CheckedCaduceDateProductService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    DateTimeProvider _dateTimeProvider;

    public CheckedCaduceDateProductService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        DateTimeProvider dateTimeProvider = new DateTimeProvider();

        var ahora = dateTimeProvider.UtcNow;
        var proximasDoce = ahora.Date.AddDays(1);
        var tiempoHastaLasDoce = proximasDoce - ahora;

        _timer = new Timer(DoWork, null, tiempoHastaLasDoce, TimeSpan.FromDays(1));

        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var _ubicacionRepository = scope.ServiceProvider.GetRequiredService<IUbicacionRepository>();
            var _movimientoRepository = scope.ServiceProvider.GetRequiredService<IMovimientoRepository>();
            var _productoRepository = scope.ServiceProvider.GetRequiredService<IProductoRepository>();
            var _dateTimeProvider = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();


            var ubicaciones_con_mermas = await _ubicacionRepository.CheckedCaduceDateProductService();

            foreach (var ubicacion in ubicaciones_con_mermas)
            {
                if (await _productoRepository.GetProductoByIdAsync(ubicacion.ID_Producto) is not Producto product)
                {
                    return;
                }

                var merma = new Movimientos
                {
                    ID_Movimiento = Guid.NewGuid(),
                    ID_Producto = ubicacion.ID_Producto,
                    Producto_Name = product.Nombre,
                    Cantidad = ubicacion.Cantidad,
                    Descripcion = "El producto ha cumplido su fecha de caducidad.",
                    Fecha = _dateTimeProvider.UtcNow,
                    Fecha_Caducidad = ubicacion.Fecha_Caducidad
                };

                await _movimientoRepository.AddMermaAsync(merma);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
