using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRUD_MVC.Models;

namespace CRUD_MVC.Controllers
{
    public class FACTURASController : Controller
    {
        private CRUDDBEntities db = new CRUDDBEntities();

        // GET: FACTURAS
        public ActionResult Index()
        {
            return View(db.FACTURAS.ToList());
        }

        // GET: FACTURAS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewData["listadoDetalles"] = db.DETALLES.Where(i => i.idFactura == id).Include(d => d.PRODUCTOS).ToList();
            FACTURAS fACTURAS = db.FACTURAS.Find(id);
            if (fACTURAS == null)
            {
                return HttpNotFound();
            }
            return View(fACTURAS);
        }

        // GET: FACTURAS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FACTURAS/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "idFactura,NumeroFactura,Fecha,TipoDePago,DocumentoCliente,NombreCliente,SubTotal,Descuento,IVA,TotalDescuento,TotalImpuesto,Total")] FACTURAS fACTURAS)
        {
            if (ModelState.IsValid)
            {
                db.FACTURAS.Add(fACTURAS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fACTURAS);
        }

        // GET: FACTURAS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FACTURAS fACTURAS = db.FACTURAS.Find(id);
            if (fACTURAS == null)
            {
                return HttpNotFound();
            }
            return View(fACTURAS);
        }

        // POST: FACTURAS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idFactura,NumeroFactura,Fecha,TipoDePago,DocumentoCliente,NombreCliente,SubTotal,Descuento,IVA,TotalDescuento,TotalImpuesto,Total")] FACTURAS fACTURAS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fACTURAS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fACTURAS);
        }

        // GET: FACTURAS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FACTURAS fACTURAS = db.FACTURAS.Find(id);
            if (fACTURAS == null)
            {
                return HttpNotFound();
            }
            return View(fACTURAS);
        }

        // POST: FACTURAS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var lista = db.DETALLES.Where(x=>x.idFactura ==id).ToList();
            for(int i=0; i<lista.Count(); i++)
            {
                db.DETALLES.Remove(lista[i]);
                db.SaveChanges();
            }

            db.FACTURAS.Remove(db.FACTURAS.Find(id));
            db.SaveChanges();
            return RedirectToAction("Index");
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
