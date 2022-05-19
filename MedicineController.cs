using MedicineService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Transactions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MedicineService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineRepository _medicineRepository;

        public MedicineController(IMedicineRepository medicineRepository)
        {
            _medicineRepository = medicineRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var medicines = _medicineRepository.GetMedicines();
            return new OkObjectResult(medicines);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var medicine = _medicineRepository.GetMedicineByID(id);
            return new OkObjectResult(medicine);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Medicine medicine)
        {
            using (var scope = new TransactionScope())
            {
                _medicineRepository.InsertMedicine(medicine);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = medicine.Id }, medicine);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] Medicine medicine)
        {
            if (medicine != null)
            {
                using (var scope = new TransactionScope())
                {
                    _medicineRepository.UpdateMedicine(medicine);
                    scope.Complete();
                    return new OkResult();
                }
            }
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _medicineRepository.DeleteMedicine(id);
            return new OkResult();
        }
    }
}
