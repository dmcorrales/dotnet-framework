using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRUD_MVC.Models;

namespace CRUD_MVC.Controllers
{
    public class DETALLESController : Controller
    {
        private CRUDDBEntities db = new CRUDDBEntities();

        // GET: DETALLES
        public ActionResult Index()
        {
            var dETALLES = db.DETALLES.Include(d => d.FACTURAS).Include(d => d.PRODUCTOS);
            return View(dETALLES.ToList());
        }

        // GET: DETALLES/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DETALLES dETALLES = db.DETALLES.Find(id);
            if (dETALLES == null)
            {
                return HttpNotFound();
            }
            return View(dETALLES);
        }

        // GET: DETALLES/Create
        public ActionResult Create()
        {
            //ViewBag.idFactura = new SelectList(db.FACTURAS, "idFactura", "TipoDePago");
            ViewData["listadoProductos"] = db.PRODUCTOS;
            ViewBag.idProducto = new SelectList(db.PRODUCTOS, "idProducto", "Producto");
            PRODUCTOS p = new PRODUCTOS
            {
                Producto = "Zapatos"
            };

            System.Diagnostics.Debug.WriteLine("This will be displayed in output window");
            ViewData["cantidadProductos"] = (int)db.PRODUCTOS.Count();
            ViewData["codigoProducto"] = db.PRODUCTOS.Select(prod => prod.idProducto).ToList();
            ViewData["nombreProducto"] = db.PRODUCTOS.Select(prod => prod.Producto).ToList();
            ViewData["valorUnitarioProducto"] = db.PRODUCTOS.Select(prod => prod.idProducto).ToList();
            return View();
        }

        // POST: DETALLES/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "idFactura,NumeroFactura,Fecha,TipoDePago,DocumentoCliente,NombreCliente,SubTotal,Descuento,IVA,TotalDescuento,TotalImpuesto,Total")] FACTURAS dFACTURAS,  int[] productoID, int[] cantidadProducto, int[] precioUnitario)
        {

            if (ModelState.IsValid)
            {
                var facturaCreada = db.FACTURAS.Add(dFACTURAS);
                db.SaveChanges();

                //Se recorre el total de registros.
                for (int i = 0; i < db.PRODUCTOS.Count(); i++)
                {
                    //sí se escogió un producto, entonces procedemos a registrarlo.
                    if (cantidadProducto[i] > 0)
                    {
                        //Nueva instancia por PRODUCTO.
                        DETALLES d = new DETALLES();
                        d.idFactura = facturaCreada.idFactura;
                        d.idProducto = i+1;
                        d.PrecioUnitario = precioUnitario[i];
                        d.Cantidad = cantidadProducto[i];
                        db.DETALLES.Add(d);
                        db.SaveChanges();
                    }

                }
            }
            return RedirectToAction("Index", "FACTURAS");


        }

        // GET: DETALLES/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DETALLES dETALLES = db.DETALLES.Find(id);
        
            if (dETALLES == null)
            {
                return HttpNotFound();
            }
            ViewBag.idFactura = new SelectList(db.FACTURAS, "idFactura", "TipoDePago", dETALLES.idFactura);
            ViewBag.idProducto = new SelectList(db.PRODUCTOS, "idProducto", "Producto", dETALLES.idProducto);
            return View(dETALLES);
        }

        // POST: DETALLES/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idDetalle,idFactura,idProducto,Cantidad,PrecioUnitario")] DETALLES dETALLES)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dETALLES).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "FACTURAS");

            }
            ViewBag.idFactura = new SelectList(db.FACTURAS, "idFactura", "TipoDePago", dETALLES.idFactura);
            ViewBag.idProducto = new SelectList(db.PRODUCTOS, "idProducto", "Producto", dETALLES.idProducto);
            return View(dETALLES);
        }

        // GET: DETALLES/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DETALLES dETALLES = db.DETALLES.Find(id);
            if (dETALLES == null)
            {
                return HttpNotFound();
            }
            return View(dETALLES);
        }

        // POST: DETALLES/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DETALLES dETALLES = db.DETALLES.Find(id);
            db.DETALLES.Remove(dETALLES);
            db.SaveChanges();
            return RedirectToAction("Index", "FACTURAS");

        }

        // GET: DETALLES/CreateById/5
        public ActionResult CreateById(int? id) {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.idProducto = new SelectList(db.PRODUCTOS, "idProducto", "Producto");
            ViewData["idFactura"] = id;
            return View();
        }

        // GET: DETALLES/CreateById/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateById([Bind(Include ="idProducto,Producto")] PRODUCTOS dPRODUCTOS, int idFactura, int precioUnitario, int cantidadProducto)
        {
            if (ModelState.IsValid)
            {
                DETALLES detalle = new DETALLES
                {
                    idFactura = idFactura,
                    idProducto = dPRODUCTOS.idProducto,
                    Cantidad = cantidadProducto,
                    PrecioUnitario = precioUnitario
                };
                db.DETALLES.Add(detalle);
                db.SaveChanges();
                return RedirectToAction("Index","FACTURAS");
            }
            ViewData["idFactura"] = idFactura;
            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
