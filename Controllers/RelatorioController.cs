using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using DotnetBackend.Services;
using MongoDB.Driver;

namespace DotnetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioController : ControllerBase
    {
        private readonly RelatorioService _relatorioService;
        private readonly ClientService _clientService;
        private readonly PurchaseService _purchaseService;
        private readonly WithdrawalService _withdrawalService;
        private readonly ConsultorService _consultorService;

        public RelatorioController(RelatorioService relatorioService, ClientService clientService,
        PurchaseService purchaseService, WithdrawalService withdrawalService, ConsultorService consultorService)
        {
            _relatorioService = relatorioService;
            _clientService = clientService;
            _purchaseService = purchaseService;
            _withdrawalService = withdrawalService;
            _consultorService = consultorService;
        }

        [HttpGet("clients/")]
        public async Task<IActionResult> GetClientReportPdf()
        {
            var clients = await _clientService.GetAllClientsAsync();
            var pdfStream = await _relatorioService.GenerateClientReportPdfAsync(clients);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_clientes.pdf");
        }


        [HttpGet("clients/consultor/{idConsultor}")]
        public async Task<IActionResult> GetClientWithConsultorReportPdf(string idConsultor)
        {
            var clients = await _clientService.GetAllClientsByConsultorIdAsync(idConsultor);
            var pdfStream = await _relatorioService.GenerateClientReportPdfAsync(clients);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_clientes_filtrados.pdf");
        }


        [HttpGet("clients/noPdf")]
        public async Task<IActionResult> GetClientReportNoPdf()
        {
            var clients = await _clientService.GetAllClientsAsync();
            return Ok(clients);
        }

        [HttpGet("clients/consultor/{idConsultor}/noPdf")]
        public async Task<IActionResult> GetClientReportNoPdf(string idConsultor)
        {
            var clients = await _clientService.GetAllClientsByConsultorIdAsync(idConsultor);
            return Ok(clients);
        }

        [HttpGet("clients/creationDate/{qttDays}")]
        public async Task<IActionResult> GetClientFilteredReportPdf(int qttDays)
        {
            var clients = await _clientService.GetAllClientsWithCreationDateFilterAsync(qttDays);
            var pdfStream = await _relatorioService.GenerateClientReportPdfAsync(clients);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_clientes_filtrados.pdf");
        }

        [HttpGet("clients/creationDate/{qttDays}/consultor/{idConsultor}")]
        public async Task<IActionResult> GetClientFilteredReportPdf(int qttDays, string idConsultor)
        {
            var clients = await _clientService.GetAllClientsWithCreationDateAndConsultorFilterAsync(qttDays, idConsultor);
            var pdfStream = await _relatorioService.GenerateClientReportPdfAsync(clients);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_clientes_filtrados.pdf");
        }

        [HttpGet("clients/creationDate/{qttDays}/noPdf")]
        public async Task<IActionResult> GetClientFilteredReportNoPdf(int qttDays)
        {
            var clients = await _clientService.GetAllClientsWithCreationDateFilterAsync(qttDays);
            return Ok(clients);
        }


        [HttpGet("clients/creationDate/{qttDays}/consultor/{idConsultor}/noPdf")]
        public async Task<IActionResult> GetClientFilteredReportNoPdf(int qttDays, string idConsultor)
        {
            var clients = await _clientService.GetAllClientsWithCreationDateAndConsultorFilterAsync(qttDays, idConsultor);
            return Ok(clients);
        }

        [HttpGet("clients/status/{status}")]
        public async Task<IActionResult> GetClientsFilteredStatusReportPdf(int status)
        {
            var clients = await _clientService.GetAllClientsAsync();
            var filteredClients = clients.Where(p => p.Status == status).ToList();
            if (!filteredClients.Any())
            {
                return NotFound("Nenhum cliente encontrada com o status especificado.");
            }

            var pdfStream = await _relatorioService.GenerateClientReportPdfAsync(filteredClients);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_clientes_filtrados.pdf");
        }

        [HttpGet("clients/status/{status}/consultor/{idConsultor}")]
        public async Task<IActionResult> GetClientsFilteredStatusReportPdf(int status, string idConsultor)
        {
            var clients = await _clientService.GetAllClientsByConsultorIdAsync(idConsultor);
            var filteredClients = clients.Where(p => p.Status == status).ToList();
            if (!filteredClients.Any())
            {
                return NotFound("Nenhum cliente encontrada com o status especificado.");
            }

            var pdfStream = await _relatorioService.GenerateClientReportPdfAsync(filteredClients);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_clientes_filtrados.pdf");
        }

        [HttpGet("clients/status/{status}/noPdf")]
        public async Task<IActionResult> GetClientsFilteredStatusReportNoPdf(int status)
        {
            var clients = await _clientService.GetAllClientsAsync();
            var filteredClients = clients.Where(p => p.Status == status).ToList();
            if (!filteredClients.Any())
            {
                return NotFound("Nenhum cliente encontrada com o status especificado.");
            }

            return Ok(filteredClients);
        }

        [HttpGet("clients/status/{status}/consultor/{idConsultor}/noPdf")]
        public async Task<IActionResult> GetClientsFilteredStatusReportNoPdf(int status, string idConsultor)
        {
            var clients = await _clientService.GetAllClientsByConsultorIdAsync(idConsultor);
            var filteredClients = clients.Where(p => p.Status == status).ToList();
            if (!filteredClients.Any())
            {
                return NotFound("Nenhum cliente encontrada com o status especificado.");
            }

            return Ok(filteredClients);
        }

        [HttpGet("clients/creationDate/{qttDays}/status/{status}")]
        public async Task<IActionResult> GetClientsFilteredStatusAndDateReportPdf(int status, int qttDays)
        {
            var clients = await _clientService.GetAllClientsWithCreationDateFilterAsync(qttDays);
            var filteredClients = clients.Where(p => p.Status == status).ToList();
            if (!filteredClients.Any())
            {
                return NotFound("Nenhum cliente encontrada com o status especificado.");
            }

            var pdfStream = await _relatorioService.GenerateClientReportPdfAsync(filteredClients);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_clientes_filtrados.pdf");
        }


        [HttpGet("clients/creationDate/{qttDays}/status/{status}/consultor/{idConsultor}")]
        public async Task<IActionResult> GetClientsFilteredStatusAndDateReportPdf(int status, int qttDays, string idConsultor)
        {
            var clients = await _clientService.GetAllClientsWithCreationDateAndConsultorFilterAsync(qttDays, idConsultor);
            var filteredClients = clients.Where(p => p.Status == status).ToList();
            if (!filteredClients.Any())
            {
                return NotFound("Nenhum cliente encontrada com o status especificado.");
            }

            var pdfStream = await _relatorioService.GenerateClientReportPdfAsync(filteredClients);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_clientes_filtrados.pdf");
        }

        [HttpGet("clients/creationDate/{qttDays}/status/{status}/noPdf")]
        public async Task<IActionResult> GetClientsFilteredStatusAndDateReportNoPdf(int status, int qttDays)
        {
            var clients = await _clientService.GetAllClientsWithCreationDateFilterAsync(qttDays);
            var filteredClients = clients.Where(p => p.Status == status).ToList();
            if (!filteredClients.Any())
            {
                return NotFound("Nenhum cliente encontrada com o status especificado.");
            }

            return Ok(filteredClients);
        }

        [HttpGet("clients/creationDate/{qttDays}/status/{status}/consultor/{idConsultor}/noPdf")]
        public async Task<IActionResult> GetClientsFilteredStatusAndDateReportNoPdf(int status, int qttDays, string idConsultor)
        {
            var clients = await _clientService.GetAllClientsWithCreationDateAndConsultorFilterAsync(qttDays, idConsultor);
            var filteredClients = clients.Where(p => p.Status == status).ToList();
            if (!filteredClients.Any())
            {
                return NotFound("Nenhum cliente encontrada com o status especificado.");
            }

            return Ok(filteredClients);
        }


        [HttpGet("consultor/")]
        public async Task<IActionResult> GetConsultorReportPdf()
        {
            var consultores = await _consultorService.GetAllConsultoresAsync();
            var pdfStream = await _relatorioService.GenerateConsultorReportPdfAsync(consultores);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_consultores.pdf");
        }

        [HttpGet("consultor/noPdf")]
        public async Task<IActionResult> GetConsultorReportNoPdf()
        {
            var consultores = await _consultorService.GetAllConsultoresAsync();
            return Ok(consultores);
        }

        [HttpGet("consultor/creationDate/{qttDays}")]
        public async Task<IActionResult> GetConsultorFilteredReportPdf(int qttDays)
        {
            var consultores = await _consultorService.GetAllConsultoresWithCreationDateFilterAsync(qttDays);
            var pdfStream = await _relatorioService.GenerateConsultorReportPdfAsync(consultores);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_consultores_filtrados.pdf");
        }

        [HttpGet("consultor/creationDate/{qttDays}/noPdf")]
        public async Task<IActionResult> GetConsultorFilteredReportNoPdf(int qttDays)
        {
            var consultores = await _clientService.GetAllClientsWithCreationDateFilterAsync(qttDays);
            return Ok(consultores);
        }

        [HttpGet("consultor/status/{status}")]
        public async Task<IActionResult> GetConsultoresFilteredStatusReportPdf(int status)
        {
            var consultores = await _consultorService.GetAllConsultoresAsync();
            var filteredConsultores = consultores.Where(p => p.Status == status).ToList();
            if (!filteredConsultores.Any())
            {
                return NotFound("Nenhum consultor encontrada com o status especificado.");
            }

            var pdfStream = await _relatorioService.GenerateConsultorReportPdfAsync(filteredConsultores);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_consultores_filtrados.pdf");
        }

        [HttpGet("consultor/status/{status}/noPdf")]
        public async Task<IActionResult> GetConsultoresFilteredStatusReportNoPdf(int status)
        {
            var clients = await _consultorService.GetAllConsultoresAsync();
            var filteredConsultores = clients.Where(p => p.Status == status).ToList();
            if (!filteredConsultores.Any())
            {
                return NotFound("Nenhum consultor encontrada com o status especificado.");
            }

            return Ok(filteredConsultores);
        }

        [HttpGet("consultor/creationDate/{qttDays}/status/{status}")]
        public async Task<IActionResult> GetConsultoresFilteredStatusAndDateReportPdf(int status, int qttDays)
        {
            var consultores = await _consultorService.GetAllConsultoresWithCreationDateFilterAsync(qttDays);
            var filteredConsultores = consultores.Where(p => p.Status == status).ToList();
            if (!filteredConsultores.Any())
            {
                return NotFound("Nenhum consultor encontrada com o status especificado.");
            }

            var pdfStream = await _relatorioService.GenerateConsultorReportPdfAsync(filteredConsultores);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_consultores_filtrados.pdf");
        }

        [HttpGet("consultor/creationDate/{qttDays}/status/{status}/noPdf")]
        public async Task<IActionResult> GetConsultoresFilteredStatusAndDateReportNoPdf(int status, int qttDays)
        {
            var consultores = await _consultorService.GetAllConsultoresWithCreationDateFilterAsync(qttDays);
            var filteredConsultores = consultores.Where(p => p.Status == status).ToList();
            if (!filteredConsultores.Any())
            {
                return NotFound("Nenhum consultor encontrada com o status especificado.");
            }

            return Ok(filteredConsultores);
        }














        [HttpGet("purchases")]
        public async Task<IActionResult> GetPurchasesReportPdf()
        {
            var purchases = await _purchaseService.GetAllPurchasesAsync();
            var pdfStream = await _relatorioService.GeneratePurchaseReportPdfAsync(purchases);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_compras.pdf");
        }

        [HttpGet("purchases/noPdf")]
        public async Task<IActionResult> GetPurchasesReportNoPdf()
        {
            var purchases = await _purchaseService.GetAllPurchasesAsync();
            return Ok(purchases);
        }

        [HttpGet("purchases/status/{status}")]
        public async Task<IActionResult> GetPurchasesFilteredStatusReportPdf(int status)
        {
            var purchases = await _purchaseService.GetAllPurchasesAsync();
            var filteredPurchases = purchases.Where(p => p.Status == status).ToList();
            if (!filteredPurchases.Any())
            {
                return NotFound("Nenhuma compra encontrada com o status especificado.");
            }

            var pdfStream = await _relatorioService.GeneratePurchaseReportPdfAsync(filteredPurchases);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_compras_filtrados.pdf");
        }

        [HttpGet("purchases/status/{status}/noPdf")]
        public async Task<IActionResult> GetPurchasesFilteredStatusReportNoPdf(int status)
        {
            var purchases = await _purchaseService.GetAllPurchasesAsync();
            var filteredPurchases = purchases.Where(p => p.Status == status).ToList();
            if (!filteredPurchases.Any())
            {
                return NotFound("Nenhuma compra encontrada com o status especificado.");
            }

            return Ok(filteredPurchases);
        }

        [HttpGet("purchases/creationDate/{qttDays}")]
        public async Task<IActionResult> GetPurchasesFilteredReportPdf(int qttDays)
        {
            var purchases = await _purchaseService.GetAllPurchasesFilteredAsync(qttDays);
            var pdfStream = await _relatorioService.GeneratePurchaseReportPdfAsync(purchases);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_compras_filtrados.pdf");
        }

        [HttpGet("purchases/creationDate/{qttDays}/noPdf")]
        public async Task<IActionResult> GetPurchasesFilteredReportNoPdf(int qttDays)
        {
            var purchases = await _purchaseService.GetAllPurchasesFilteredAsync(qttDays);
            return Ok(purchases);
        }

        [HttpGet("purchases/creationDate/{qttDays}/status/{status}")]
        public async Task<IActionResult> GetPurchasesFilteredStatusAndDateReportPdf(int status, int qttDays)
        {
            var purchases = await _purchaseService.GetAllPurchasesFilteredAsync(qttDays);
            var filteredPurchases = purchases.Where(p => p.Status == status).ToList();
            if (!filteredPurchases.Any())
            {
                return NotFound("Nenhuma compra encontrada com o status especificado.");
            }

            var pdfStream = await _relatorioService.GeneratePurchaseReportPdfAsync(filteredPurchases);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_compras_filtrados.pdf");
        }

        [HttpGet("purchases/creationDate/{qttDays}/status/{status}/noPdf")]
        public async Task<IActionResult> GetPurchasesFilteredStatusAndDateReportNoPdf(int status, int qttDays)
        {
            var purchases = await _purchaseService.GetAllPurchasesFilteredAsync(qttDays);
            var filteredPurchases = purchases.Where(p => p.Status == status).ToList();
            if (!filteredPurchases.Any())
            {
                return NotFound("Nenhuma compra encontrada com o status especificado.");
            }

            return Ok(filteredPurchases);
        }

        [HttpGet("withdrawals")]
        public async Task<IActionResult> GetWithdrawalsReportPdf()
        {
            var withdrawals = await _withdrawalService.GetAllWithdrawalsAsync();
            var pdfStream = await _relatorioService.GenerateWithdrawalReportPdfAsync(withdrawals);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_saques.pdf");
        }

        [HttpGet("withdrawals/noPdf")]
        public async Task<IActionResult> GetWithdrawalsReportNoPdf()
        {
            var withdrawals = await _withdrawalService.GetAllWithdrawalsAsync();
            return Ok(withdrawals);
        }

        [HttpGet("withdrawals/creationDate/{qttDays}")]
        public async Task<IActionResult> GetWithdrawalFilteredReportPdf(int qttDays)
        {
            var withdrawals = await _withdrawalService.GetAllWithdrawalsFilteredAsync(qttDays);
            var pdfStream = await _relatorioService.GenerateWithdrawalReportPdfAsync(withdrawals);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_saques_filtrados.pdf");
        }

        [HttpGet("withdrawals/creationDate/{qttDays}/noPdf")]
        public async Task<IActionResult> GetWithdrawalFilteredReportNoPdf(int qttDays)
        {
            var withdrawals = await _withdrawalService.GetAllWithdrawalsFilteredAsync(qttDays);
            return Ok(withdrawals);
        }

        [HttpGet("withdrawals/status/{status}")]
        public async Task<IActionResult> GetWithdrawalsFilteredStatusReportPdf(int status)
        {
            var withdrawals = await _withdrawalService.GetAllWithdrawalsAsync();
            var filteredWithdrawals = withdrawals.Where(p => p.Status == status).ToList();
            if (!filteredWithdrawals.Any())
            {
                return NotFound("Nenhum saque encontrado com o status especificado.");
            }

            var pdfStream = await _relatorioService.GenerateWithdrawalReportPdfAsync(withdrawals);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_saques_filtrados.pdf");
        }

        [HttpGet("withdrawals/status/{status}/noPdf")]
        public async Task<IActionResult> GetWithdrawalsFilteredStatusReportNoPdf(int status)
        {
            var withdrawals = await _withdrawalService.GetAllWithdrawalsAsync();
            var filteredWithdrawals = withdrawals.Where(p => p.Status == status).ToList();
            if (!filteredWithdrawals.Any())
            {
                return NotFound("Nenhum saque encontrado com o status especificado.");
            }
            return Ok(filteredWithdrawals);
        }

        [HttpGet("withdrawals/creationDate/{qttDays}/status/{status}")]
        public async Task<IActionResult> GetWithdrawalsFilteredStatusAndDateReportPdf(int status, int qttDays)
        {
            var withdrawals = await _withdrawalService.GetAllWithdrawalsFilteredAsync(qttDays);
            var filteredWithdrawals = withdrawals.Where(p => p.Status == status).ToList();
            if (!filteredWithdrawals.Any())
            {
                return NotFound("Nenhum saque encontrado com o status especificado.");
            }

            var pdfStream = await _relatorioService.GenerateWithdrawalReportPdfAsync(filteredWithdrawals);
            return File(pdfStream.ToArray(), "application/pdf", "relatorio_saques_filtrados.pdf");
        }

        [HttpGet("withdrawals/creationDate/{qttDays}/status/{status}/noPdf")]
        public async Task<IActionResult> GetWithdrawalsFilteredStatusAndDateReportNoPdf(int status, int qttDays)
        {
            var withdrawals = await _withdrawalService.GetAllWithdrawalsFilteredAsync(qttDays);
            var filteredWithdrawals = withdrawals.Where(p => p.Status == status).ToList();
            if (!filteredWithdrawals.Any())
            {
                return NotFound("Nenhum saque encontrado com o status especificado.");
            }
            return Ok(filteredWithdrawals);
        }
    }
}