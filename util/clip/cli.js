#! /usr/bin/env node

process.stdin.setEncoding('utf8');

process.stdin.on('data', function (data) {
	data = data.split('\n');
	data = data.map((v) => {
		return v.split(':')[0];
	});
	data = data.join(' ');
	// data = data.trim(',');
	console.log(data);
	
});
