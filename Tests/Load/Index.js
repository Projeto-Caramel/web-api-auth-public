import GetPets from "./Scenarios/Pets/get-pets.js";
import GetPartners from "./Scenarios/Partners/get-partners.js";
import {group, sleep} from 'k6';
import getPartners from "./Scenarios/Partners/get-partners.js";

export default () => {           
    group('Endpoint Get Customer Controller Customer OnionArchitecture.Api', () => {
        // getGetCustomer();
        GetPartners();
    });

    sleep(1);
}

