package main

import (
	"bufio"
	"bytes"
	"fmt"
	"os"
	"regexp"
	"strconv"
	"strings"
)

type tickets struct {
	fields fields
	mine   ticket
	nearby []ticket
}
type fields []field
type field struct {
	name   string
	values []int
}
type ticket []int

func main() {
	tickets := prepare("input.txt")
	fmt.Println("part one:", sum(tickets.invalid()))
	fmt.Println("part two:", mul(tickets.departure()))
}
func prepare(fn string) (t tickets) {
	part := 0

	file, _ := os.Open(fn)
	defer file.Close()
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := scanner.Bytes()

		switch string(line) {
		case "your ticket:":
			part = 1
			continue
		case "nearby tickets:":
			part = 2
			continue
		case "":
			continue
		}

		switch part {
		case 0: // Fields
			pattern := regexp.MustCompile(`([a-z\s]+): ([0-9]+)-([0-9]+) or ([0-9]+)-([0-9]+)`)
			matches := pattern.FindAllSubmatch(line, -1)

			field := field{
				name: string(matches[0][1]),
			}
			for i := parseInt(matches[0][2]); i <= parseInt(matches[0][5]); i++ {
				if i <= parseInt(matches[0][3]) || i >= parseInt(matches[0][4]) {
					field.values = append(field.values, i)
				}
			}
			t.fields = append(t.fields, field)
			break
		case 1: // My ticket
			for _, b := range bytes.Split(line, []byte(",")) {
				t.mine = append(t.mine, parseInt(b))
			}
			break
		case 2: // Nearby tickets
			values := []int{}
			for _, b := range bytes.Split(line, []byte(",")) {
				values = append(values, parseInt(b))
			}
			t.nearby = append(t.nearby, values)
			break
		}
	}
	return
}
func (t *tickets) invalid() (values []int) {
	for _, ticket := range t.nearby {
		if f := ticket.invalid(t.fields); len(f) > 0 {
			values = append(values, f...)
		}
	}

	return
}
func (t *tickets) departure() (values []int) {
	valid := []ticket{}
	valid = append(valid, t.mine)

	for _, ticket := range t.nearby {
		if f := ticket.invalid(t.fields); len(f) == 0 {
			valid = append(valid, ticket)
		}
	}

	positioned := t.fields.position(valid)

	for k, v := range positioned {
		if strings.HasPrefix(k, "departure") {
			values = append(values, t.mine[v])
		}
	}

	return
}
func (f fields) position(tickets []ticket) (positioned map[string]int) {
	positioned = make(map[string]int, 0)

	possibles := make(map[int][]field, len(f))
	for _, field := range f {
		for pos := 0; pos < len(f); pos++ {
			valid := true
			for _, t := range tickets {
				tv := &ticket{t[pos]}
				if v := tv.invalid(fields{field}); len(v) != 0 {
					valid = false
					break
				}
			}

			if valid {
				possibles[pos] = append(possibles[pos], field)
			}
		}
	}

	for {
		for pos, fields := range possibles {
			unmatched := []string{}
			for _, f := range fields {
				if _, ok := positioned[f.name]; !ok {
					unmatched = append(unmatched, f.name)
				}
			}
			if len(unmatched) == 1 {
				positioned[unmatched[0]] = pos
			}
		}
		if len(positioned) == len(f) {
			break
		}
	}

	return
}
func (t *ticket) invalid(f fields) (invalid []int) {
	fv := []int{}
	for _, field := range f {
		fv = append(fv, field.values...)
	}
	v := make(map[int]struct{})
	for _, i := range fv {
		v[i] = struct{}{}
	}
	for _, i := range *t {
		if _, ok := v[i]; !ok {
			invalid = append(invalid, i)
		}
	}

	return
}
func sum(values []int) (sum int) {
	for _, value := range values {
		sum += value
	}
	return
}
func mul(values []int) (mul int) {
	mul = 1
	for _, value := range values {
		mul *= value
	}
	return
}
func parseInt(b []byte) int {
	i, _ := strconv.Atoi(string(b))
	return i
}
