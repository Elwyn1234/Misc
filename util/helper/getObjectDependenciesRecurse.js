const fs = require("fs");
const jsYaml = require("js-yaml");

module.exports.getObjectDependenciesRecurse = (object, vars) => {
    for (let i = 0; i < object.dependencies.length; i++) {
        const depId = object.dependencies[i].id;

        if (depId.startsWith("D610")) {
            const filePath = `/NIRS2/dev/git/Application/usr/src/${depId.toLowerCase()}.yaml`;
            if (!fs.existsSync(filePath))
                throw new Error(`${filePath} does not exist`);
            const dep = jsYaml.load(fs.readFileSync(filePath, "utf8"));
            module.exports.getObjectDependenciesRecurse(dep, vars);
        	  vars.push(depId);
            continue;
        }

        if (!depId.startsWith("D710") && !depId.startsWith("RATTD"))
            throw new Error("Unknown Type: ", depId);

        vars.push(depId);
    }
}

// Returns an object holding usr file variables as well as the id (eg: IF54025)
module.exports.loadObjects = (objects) => {
    usrs = new Array(objects.length);
    for (let i = 0; i < objects.length; i++) {
        const filePath = `/NIRS2/dev/git/Application/usr/src/d610.${objects[i].toLowerCase()}.yaml`;
        if (!fs.existsSync(filePath))
            throw new Error(`${filePath} does not exist`);
    
        usrs[i] = jsYaml.load(fs.readFileSync(filePath, "utf8"));
        usrs[i].id = objects[i];
    }
    return usrs;
}
