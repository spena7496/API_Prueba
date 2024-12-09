using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductosController : ControllerBase
    {

        private static List<Producto> _productos = new List<Producto>();

        private readonly ILogger<ProductosController> _logger;

        public ProductosController(ILogger<ProductosController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetAllProductos")]

        public List<Producto> Get()
        {
            return _productos;
        }
        [HttpPost(Name = "CreateProducto")]
 
        public IActionResult CreateProducto(Producto oProducto) 
        {
            Producto producto = _productos.FirstOrDefault(x => x.Id == oProducto.Id);
            if ( producto == null)
            {
                _productos.Add(oProducto);
                return CreatedAtAction(nameof(CreateProducto), new { id = oProducto.Id }, oProducto);
            }
            else
            {
               return BadRequest(new { mensaje = "Ya existe un producto con ese id." });
            }

        }
        [HttpPut(Name = "UpdateProducto")]

        public IActionResult UpdateProducto(Producto oProducto)
        {
            Producto producto = _productos.Where(x => x.Id == oProducto.Id).FirstOrDefault();
            if(producto!= null){
                _productos.Remove(producto);
                _productos.Add(oProducto);
                return CreatedAtAction(nameof(UpdateProducto), new { id = oProducto.Id }, oProducto);
            }
            else
            {
                return BadRequest(new { mensaje = "No se puede actualizar un producto que no existe" });
            }
           
        }
        [HttpDelete(Name = "DeleteProducto")]
 
        public IActionResult DeleteProducto(int idProducto)
        {
            Producto producto = _productos.Where(x => x.Id == idProducto).FirstOrDefault();
            if(producto==null)
            {
                return BadRequest(new { mensaje = "No se puede eliminar un producto que no existe" });
            }
            else
            {
                _productos.Remove(_productos.Where(x => x.Id == idProducto).FirstOrDefault());
                return CreatedAtAction(nameof(UpdateProducto), new { id = idProducto }, idProducto);
            }
           
        }

    }
}
