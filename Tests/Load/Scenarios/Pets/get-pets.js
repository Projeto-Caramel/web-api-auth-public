import http from 'k6/http';
import { sleep } from 'k6';
import { Trend, Rate, Counter } from "k6/metrics";
import { check, fail } from "k6";

export let GetPartnerDuration = new Trend('get_customer_duration');
export let GetPartnerFailRate = new Rate('get_customer_fail_rate');
export let GetPartnerSuccessRate = new Rate('get_customer_success_rate');
export let GetPartnerReqs = new Rate('get_customer_reqs');

export default function () {
    let res = http.get('https://localhost:7127/api/partners?page=1&pageSize=10');

    GetPartnerDuration.add(res.timings.duration);
    GetPartnerReqs.add(1);

    GetPartnerFailRate.add(res.status == 0 || res.status > 399);
    GetPartnerSuccessRate.add(res.status < 399);

    let durationMsg = 'Max Duration ${4000/1000}s';
    if (!check(res, {
        'max duration': (r) => r.timings.duration < 4000,
    })) {
        fail(durationMsg);
    }

    sleep(1);
}
