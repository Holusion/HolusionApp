'use strict';
const {resolve} = require("path");
const {promises:fs} = require("fs");
const fetch = require("node-fetch");
const parse = require('csv-parse/lib/sync')

const doc_url = "https://docs.google.com/spreadsheets/d/1h4L_wfsOJHyA5pUO8dOBUfto_ULnFj7lnIYw2Kas79k";

const get_sheet = (name) => `${doc_url}/gviz/tq?tqx=out:csv&sheet=${encodeURIComponent(name)}`;

let sheets = ["Session1", "Session2", "Session3", "Session4", "Session5"];

let jsonDir = resolve(__dirname, "JSON");

const getBlock = (row) => {
  let res = ["text","audio","gesture","face"].reduce((r,k)=>((row[k] && row[k].length)?{...r, [k]:row[k]} : r),{});
  if(!res.text){
    res.text = `${res.gesture?`[${res.gesture}]`:""}${res.face?`[${res.face}]`:""}`
  }
  return res;
};

(async ()=>{
  await fs.rm(jsonDir, {recursive: true, force: true});
  await fs.mkdir(jsonDir);

  for(let sheet of sheets){
    console.log("Fetching ", sheet);
    const r = await fetch(get_sheet(sheet));
    if(!r.ok){
      console.log("Failed to get sheet", sheet, " : ", r.statusText);
      continue;
    }
    const content = await r.text();
    const rows = parse(content, {
      columns: true,
      skip_empty_lines: true
    });
    let res = {stages:[]}, part_ptr;
    for(let [lineNb, row] of rows.entries()){
      switch(row.identifier){
        case "title":
          if(res[row.text]) throw new Error(`Duplicate title ${JSON.stringify(row)} for sheet ${sheet}`);
          res.stages.push({title: row.text, blocks: []});
          part_ptr = res.stages.slice(-1)[0].blocks;
          break;
        case "block":
          part_ptr.push(getBlock(row));
          break;
        case "block_m":
        case "block_f":
          break;
        default:
          let m = /^F(\d)$/.exec(row.identifier);
          if(!m) throw new Error(`Invalid identifier : ${JSON.stringify(row)} on sheet ${sheet}, line ${lineNb}`);
          const last_block_ptr = part_ptr[part_ptr.length -1];
          if(!last_block_ptr) throw new Error(`no last block to refer to on line ${lineNb} for sheet ${sheet}`);
          if(!last_block_ptr.choices) last_block_ptr.choices = [];
          let choiceNb = parseInt(m[1]) -1;
          if(!last_block_ptr.choices[choiceNb]){
            last_block_ptr.choices[choiceNb] = {
              btn: row.choice,
              score: row.score?parseInt(row.score):0,
              res: [getBlock(row)]
            }
          }else{
            last_block_ptr.choices[choiceNb].res.push(getBlock(row));
          }
      }
    }
    await fs.writeFile(resolve(jsonDir, `${sheet}.json`), JSON.stringify(res, null, 2));
  }
})().then(()=>{
  console.log("done.");
  process.exit(0);
}, (e)=>{
  console.error(e);
  process.exit(1);
})