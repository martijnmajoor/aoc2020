import fs from "fs";
import readline from "readline";

const required = ["byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"];
const validators = {
  byr: (val) => {
    return val >= 1920 && val <= 2002;
  },
  iyr: (val) => {
    return val >= 2010 && val <= 2020;
  },
  eyr: (val) => {
    return val >= 2020 && val <= 2030;
  },
  hgt: (val) => {
    const match = val.match(/^([0-9]{2,3})(cm|in)$/i),
      [min, max] = ((t) => {
        switch (t) {
          case "cm":
            return [150, 193];
          case "in":
            return [59, 76];
          default:
            return [-1, -1];
        }
      })(match?.[2]);
    return match?.[1] >= min && match?.[1] <= max;
  },
  hcl: (val) => {
    return RegExp(/^#[0-9a-f]{6}$/i).test(val);
  },
  ecl: (val) => {
    return ["amb", "blu", "brn", "gry", "grn", "hzl", "oth"].includes(val);
  },
  pid: (val) => {
    return RegExp(/^[0-9]{9}$/).test(val);
  },
};

const lineReader = readline.createInterface({
  input: fs.createReadStream("input.txt"),
});

const validate = (line) => {
  if (!line) {
    if (fields.length == required.length) {
      valid++;
    }
    if (fieldsStrict.length == required.length) {
      validStrict++;
    }

    fields = [];
    fieldsStrict = [];
  } else {
    line.split(" ").forEach((pair) => {
      const [k, v] = pair.split(":");

      if (required.includes(k) && !fields.includes(k)) {
        fields.push(k);

        if (validators[k] && validators[k](v)) {
          fieldsStrict.push(k);
        }
      }
    });
  }
};

let valid = 0;
let validStrict = 0;
let fields = [];
let fieldsStrict = [];

lineReader.on("line", validate);
lineReader.on("close", () => {
  validate("");

  console.log(`
    part one: ${valid}\n
    part two: ${validStrict}
  `);
});
