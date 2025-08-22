using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demostracion_POO.Controllers
{
    //Abstracción: contrato común
    public abstract class Vehiculo
    {
        public string Marca { get; }
        protected Vehiculo(string marca) => Marca = marca;

        public abstract string Encender();
        public virtual string Describir() => $"Vehículo genérico de marca {Marca}.";
    }

    // Herencia: Auto y Moto derivan de Vehiculo
    public class Auto : Vehiculo
    {
        public Auto(string marca) : base(marca) { }
        public override string Encender() => $"Auto {Marca}: motor encendido (llave o botón).";
        public override string Describir() => $"Auto de marca {Marca}.";
    }

    public class Moto : Vehiculo
    {
        public Moto(string marca) : base(marca) { }
        public override string Encender() => $"Moto {Marca}: motor encendido (patada o switch).";
        public override string Describir() => $"Moto de marca {Marca}.";
    }

    // Encapsulación: control de acceso al estado interno
    public class CuentaBancaria
    {
        private decimal _saldo;
        public string Codigo { get; }
        public string Titular { get; }

        public decimal Saldo => _saldo; // solo lectura hacia afuera

        public CuentaBancaria(string codigo, string titular, decimal saldoInicial = 0m)
        {
            if (saldoInicial < 0) throw new ArgumentException("El saldo inicial no puede ser negativo.");
            Codigo = codigo;
            Titular = titular;
            _saldo = saldoInicial;
        }

        public void Depositar(decimal monto)
        {
            if (monto <= 0) throw new ArgumentException("El depósito debe ser mayor a cero.");
            _saldo += monto;
        }

        public void Retirar(decimal monto)
        {
            if (monto <= 0) throw new ArgumentException("El retiro debe ser mayor a cero.");
            if (monto > _saldo) throw new InvalidOperationException("Fondos insuficientes.");
            _saldo -= monto;
        }
    }

    // DTOs para respuestas de Swagger
    public record CuentaDto(string Codigo, string Titular, decimal Saldo);
    public record MensajeDto(string Mensaje);

    // ====== CONTROLLER CON ENDPOINTS ====== //
    [Route("api/[controller]")]
    [ApiController]
    public class PooController : ControllerBase
    {

        // "Estado" de ejemplo para encapsulación (a modo demo).
        private static readonly CuentaBancaria _cuenta = new CuentaBancaria("PE01", "Bruce Wayne", 100m);

        /// <summary>
        /// Abstracción: usar una clase base abstracta (Vehiculo) sin conocer detalles concretos.
        /// </summary>
        [HttpGet("abstraccion")]
        [ProducesResponseType(typeof(MensajeDto), 200)]
        public ActionResult<MensajeDto> Abstraccion()
        {
            Vehiculo v = new Auto("Toyota"); // Trabajamos con el tipo abstracto
            return Ok(new MensajeDto(v.Encender()));
        }

        /// <summary>
        /// Encapsulación: depositar en una cuenta sin exponer directamente el saldo interno.
        /// </summary>
        [HttpPost("encapsulacion/depositar")]
        [ProducesResponseType(typeof(CuentaDto), 200)]
        [ProducesResponseType(400)]
        public ActionResult<CuentaDto> Depositar([FromQuery] decimal monto)
        {
            try
            {
                _cuenta.Depositar(monto);
                return Ok(new CuentaDto(_cuenta.Codigo, _cuenta.Titular, _cuenta.Saldo));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Encapsulación: retirar de una cuenta validando reglas internas.
        /// </summary>
        /// 
        [HttpPost("encapsulacion/retirar")]
        [ProducesResponseType(typeof(CuentaDto), 200)]
        [ProducesResponseType(400)]
        public ActionResult<CuentaDto> Retirar([FromQuery] decimal monto)
        {
            try
            {
                _cuenta.Retirar(monto);
                return Ok(new CuentaDto(_cuenta.Codigo, _cuenta.Titular, _cuenta.Saldo));
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Herencia: mostrar descripciones específicas de clases derivadas.
        /// </summary>
        [HttpGet("herencia")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        public ActionResult<IEnumerable<string>> Herencia()
        {
            var auto = new Auto("Ford");
            var moto = new Moto("Yamaha");
            return Ok(new[] { auto.Describir(), moto.Describir() });
        }

        /// <summary>
        /// Polimorfismo: la misma llamada (Encender) se comporta distinto según el tipo concreto.
        /// </summary>
        [HttpGet("polimorfismo")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        public ActionResult<IEnumerable<string>> Polimorfismo()
        {
            var vehiculos = new List<Vehiculo>
            {
                new Auto("Mazda"),
                new Moto("Honda"),
                new Auto("Kia"),
                new Moto("Suzuki")
            };

            var resultados = vehiculos.Select(v => v.Encender()).ToList();
            return Ok(resultados);
        }

    }
}
