import fs from "fs";
import readline from "readline";

// I would not recommend running the example from part one when solving part two...
const f = "example.txt";

const lineReader = readline.createInterface({
  input: fs.createReadStream(f),
});

const override = (val, mask) => {
  let bits = parseBits(val, mask.length);

  mask.split("").forEach((val, idx) => {
    if (val != "X") {
      bits = replaceAt(bits, idx, val);
    }
  });
  return parseInt(bits, 2);
};
const decode = (addr, value, mask, mem) => {
  let bits = parseBits(addr, mask.length);

  mask.split("").forEach((val, idx) => {
    if (val != "0") {
      bits = replaceAt(bits, idx, val);
    }
  });

  let targets = new Set();

  float(bits, targets);

  targets.forEach((target) => {
    mem[target] = parseInt(value, 10);
  });
};
const float = (bits, targets) => {
  const idx = bits.indexOf("X");
  if (idx == -1) {
    targets.add(parseInt(bits, 2));
  } else {
    float(replaceAt(bits, idx, 0), targets);
    float(replaceAt(bits, idx, 1), targets);
  }
};
const parseBits = (val, pad) => {
  return parseInt(val, 10).toString(2).padStart(pad, "0");
};
const replaceAt = (str, pos, repl) => {
  return `${str.substr(0, pos)}${repl}${str.substr(pos + 1)}`;
};
const sum = (obj) => {
  return Object.values(obj).reduce((acc, val) => {
    return (acc += val);
  }, 0);
};

let mask;
let memOne = {};
let memTwo = {};

lineReader.on("line", (line) => {
  if (line.startsWith("mask")) {
    mask = line.substr(7);
    return;
  }

  const instr = line.split(/mem\[(\d+)\] = (\d+)/);

  memOne[instr[1]] = override(instr[2], mask);
  decode(instr[1], instr[2], mask, memTwo);
});

lineReader.on("close", () => {
  console.log(`
    part one: ${sum(memOne)}\n
    part two: ${sum(memTwo)}
  `);
});
