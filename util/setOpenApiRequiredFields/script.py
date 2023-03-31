#!/usr/bin/env python2
import yaml

f = file('config.txt', 'r')
bfms = []
while (True):
  line = f.readline()
  if (not line): break

  bfms.append(line.strip('\n'))

print(bfms)

for bfm in bfms:
  spec = yaml.load('/NIRS2/dev/git/NPS/Builds/NIAPIs/yaml/openapi_t235bfm.' + bfm + '.yaml')
  print("1: " + spec)
  print(spec[info])

"""
1. Task remove redundant and add required
  1) Read OpenAPI spec and analysis
  2) Get Union of minimally populated requests
  3) Set required fields for each object
    a. For each object
      i. Find the object in the tree
      ii. Add required array and push each required field onto that array for the object




  4) get fully populated request from overlay_examples or from ade's script output
  5) Remove fields that are unused
    a. Recursively for each object
      i. Compare analysis to OpenAPI spec
      ii. For fields that are missing in the analysis
        1) Remove this field from the yaml
  6) Write out updated OpenAPI spec
"""
