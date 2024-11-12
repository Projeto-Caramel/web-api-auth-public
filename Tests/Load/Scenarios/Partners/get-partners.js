import http from 'k6/http';
import { sleep } from 'k6';
import { Trend, Rate, Counter } from 'k6/metrics';

// Definindo métricas
let getPartnersDuration = new Trend('get_partners_duration');
let getPartnersFailRate = new Rate('get_partners_fail_rate');
let getPartnersSuccessRate = new Rate('get_partners_success_rate');
let getPartnersRequests = new Counter('get_partners_requests');

export default function () {
    const pagination = {
        Page: 1,
        Size: 10,
    };

    const options = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    // Fazendo a solicitação GET para a rota GetPartners com o objeto Pagination no corpo
    let response = http.get('http://localhost:7127/api/v1/partners', {
        params: pagination, options
    });

    console.log(response);
    console.log("\n");

    // Registrando a duração da solicitação
    getPartnersDuration.add(response.timings.duration);

    // Registrando o resultado da solicitação (sucesso ou falha)
    if (response.status >= 200 && response.status < 400) {
        getPartnersSuccessRate.add(1);
    } else {
        getPartnersFailRate.add(1);
    }

    // Contando o número total de solicitações
    getPartnersRequests.add(1);

    // Esperando por um curto período antes da próxima solicitação
    sleep(1);
}
