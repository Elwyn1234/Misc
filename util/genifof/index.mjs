#!/home/ejoh/.n/bin/node

import fs from 'fs';
import yaml from 'js-yaml'
import usrman from '@devtools/usr-manager';
import loadfs from '@devtools/usr-manager-load-fs';
import helper from '../helper/getObjectDependenciesRecurse.js';

const npsConfig = new usrman.NPSConfigContext();
npsConfig.registerLoad(loadfs.loadFromUSRDirectories);
npsConfig.load([`/NIRS2/dev/git/Application/usr/src`]);
if (!npsConfig.validateAgainstSchemas())
  throw new Error(`Schema validation failed`);
npsConfig.mapDependencies();

const file = fs.readFileSync("/home/ejoh/util/genifof/d610s.txt", "utf8");
const bfms = file.split('\n');

const expandedObjs = [];
bfms.forEach((bfm) => {
  if (bfm === "") return;
  let obj = {};
  obj.in = usrman.expandObject(npsConfig, "D610.IF" + bfm, false, false);
  obj.out = usrman.expandObject(npsConfig, "D610.OF" + bfm, false, false);
  obj.id = bfm;
  expandedObjs.push(obj);
});

let bigObject = {};
let jsonout = "";
let yamlout = "";
let ifofcsvout = "";
let bfmcsvout = "";
expandedObjs.forEach((obj) => {
  yamlout += yaml.dump(obj.in + '\n' + obj.out) + '\n';

  const csvobj = {};
  Object.keys(obj.in).forEach((key) => {
    bigObject[key] = obj.in[key];
    csvobj[key] = obj.in[key];
  }); 
  Object.keys(obj.out).forEach((key) => {
    bigObject[key] = obj.out[key];
    csvobj[key] = obj.out[key];
  }); 
  ifofcsvout += "BF" + obj.id + ',' + JSON.stringify(csvobj, null, 2) + '\n';

  const inputDependencies = new Array();
  const outputDependencies = new Array();
  const usrObjContextInput = helper.loadObjects(["IF" + obj.id]);
  const usrObjContextOutput = helper.loadObjects(["OF" + obj.id]);
  helper.getObjectDependenciesRecurse(usrObjContextInput[0], inputDependencies);
  helper.getObjectDependenciesRecurse(usrObjContextOutput[0], outputDependencies);
  console.log(outputDependencies);
  const npsConfigObject = npsConfig.findById("D610.IF" + obj.id);
  bfmcsvout += "BF" + obj.id + ', IF' + obj.id + ',' + inputDependencies.join(',') + '\n'
    + ',OF' + obj.id + ',' + outputDependencies.join(',') + '\n';
});
jsonout = JSON.stringify(bigObject, null, 2);

fs.writeFileSync("IFOFStructures.json", jsonout, "utf8");
fs.writeFileSync("IFOFStructures.yaml", yamlout, "utf8");
fs.writeFileSync("IFOFStructures.csv", ifofcsvout, "utf8");
fs.writeFileSync("BFMBreakdown.csv", bfmcsvout, "utf8");


// TODO:
// - Place corresponding IF OF structs next to each other in output
// - Repull Application before final run
