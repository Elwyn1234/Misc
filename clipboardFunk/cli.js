/*
import clipboard from 'clipboardy';

const clip = "vim " + clipboard.readSync().split(".")[1];
clipboard.writeSync(clip);
*/

import fs from "fs";
import child_process from "child_process";

const dir = fs.opendirSync("./")
let dirEntry;
let successes = [];
// warns are strings
let warns = [];
// fails are exceptions
let fails = [];
let out;
while (dirEntry = dir.readSync()) {
    try {
        out = child_process.execSync(`openapi lint ${dirEntry.name} 2>&1`, { encoding:"utf8"});
    } catch (e) {
        fails.push(e);
        continue;
    }

    if (out.match("warning"))
        warns.push(out);
    else
        successes.push(out);
}

// warns are strings
console.log("\n\n################################################ Warnings: ################################################\n\n");
warns.forEach((v) => {
    console.log(v);
});

console.log("\n\n################################################ Failures: ################################################\n\n");
fails.forEach((v) => {
    console.log(v.output[1]);
});

// fails are exceptions
console.log(`number of fails: ${fails.length}`);
// warns are strings. commented out because warnings dont seem to be working 
console.log(`number of warnings: ${warns.length}`);
console.log(`number of successes: ${successes.length}`);
