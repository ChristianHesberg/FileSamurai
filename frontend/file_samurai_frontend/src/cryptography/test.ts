import fs from "node:fs";
import {createFile} from "./create-file.cryptography";
import {decryptFile} from "./decrypt-file.cryptography";
import {createUserKeyPair} from "./user-registers.cryptography";
import {shareFile} from "./share-file.cryptography";
import {VIEWER_ROLE} from "../constants";

async function test(){
    fs.readFile('test.txt', async (err, data) => {
        if(err) console.log(err);
        if(data) await createFile('e9171352-c0e3-4705-8f52-5afca618c8b2', 'f42eb234-8f11-4339-b6c5-7aca9a9091be', data, 'COOL TITLE');
    })
}

//createUserKeyPair('very_secret_password_that_you_cannot_guess', 'lame@email.com', 'c3ffaafd-3ee1-482b-9c39-99768c1d07db');
//test();
//decryptFile('e9171352-c0e3-4705-8f52-5afca618c8b2', '3a25fdfb-1b73-47de-8cea-b40b18b6b93a', 'cool@email.com-very_secret_password_that_you_cannot_guess');
//shareFile('e9171352-c0e3-4705-8f52-5afca618c8b2', 'c3ffaafd-3ee1-482b-9c39-99768c1d07db', '3a25fdfb-1b73-47de-8cea-b40b18b6b93a', 'cool@email.com-very_secret_password_that_you_cannot_guess', VIEWER_ROLE);
