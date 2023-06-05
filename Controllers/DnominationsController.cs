using Microsoft.AspNetCore.Mvc;

namespace testCode.Controllers;

[Route("[controller]")]
public class DenominationsController: Controller
{
   private readonly ILogger<DenominationsController> _logger;

    public DenominationsController(ILogger<DenominationsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("GetCombinations/{Amount}")]
    public JsonResult GetCombinations(int Amount)
    {
        if(Amount < 0)
        {
            return Json(new { message = "Amount cannot be negative" });
        }
        int[] Denominations = new int[] { 10, 50, 100 };
        return Json(new { message = "The number of possible combinations with 10, 50 and 100 for " + Amount + " are: " + GetDenominationCombinations(Amount, Denominations) });
    }

    public static int GetDenominationCombinations(int amount, int[] denominations)
    {
        int[] combinations = new int[amount + 1];
        combinations[0] = 1;

        foreach (int denomination in denominations)
        {
            for (int i = denomination; i <= amount; i=i+(denomination))
            {
                combinations[i] += combinations[i - denomination];
            }
        }

        return combinations[amount];
    }
}