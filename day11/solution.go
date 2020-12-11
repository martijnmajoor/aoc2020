package main

import (
	"bufio"
	"bytes"
	"crypto/md5"
	"encoding/hex"
	"fmt"
	"math"
	"os"
)

const fn = "example.txt"

func main() {
	fmt.Println("part one:", seat(4))
	fmt.Println("part two:", seat(5))
}

func seat(tolerance int) int {
	layout := file(fn)
	rows := int(math.Sqrt(float64(len(layout))))

	prevHash := hash(layout)
	for {
		layout = shuffle(layout, rows, tolerance)

		curHash := hash(layout)
		if curHash == prevHash {
			break
		}
		prevHash = curHash
	}

	return bytes.Count(layout, []byte{'#'})
}
func shuffle(layout []byte, rows int, tolerance int) (result []byte) {
	result = make([]byte, len(layout))
	copy(result, layout)

	for seat := range layout {
		var amount int
		if tolerance == 4 {
			amount = adjacent(layout, rows, seat)
		} else {
			amount = visible(layout, rows, seat)
		}

		if layout[seat] == 'L' && amount == 0 {
			result[seat] = '#'
		} else if layout[seat] == '#' && amount >= tolerance {
			result[seat] = 'L'
		}
	}

	return
}
func adjacent(layout []byte, rows int, seat int) (amount int) {
	self := seat % rows

	for i := max(seat-rows-1, 0); i <= min(seat+rows+1, len(layout)-1); i++ {
		other := i % rows

		if i != seat && other >= self-1 && other <= self+1 && layout[i] == '#' {
			amount++
		}
	}
	return
}
func visible(layout []byte, rows int, seat int) int {
	see := make([]byte, 8)
	for i := range see {
		see[i] = '.'
	}

	for i := 1; i < rows; i++ {
		moves := []int{
			i,           // RIGHT
			rows*i + i,  // BOTTOM RIGHT
			-rows*i + i, // TOP RIGHT
			-rows * i,   // TOP
			rows * i,    // BOTTOM
			-i,          // LEFT
			rows*i - i,  // BOTTOM LEFT
			-rows*i - i, // TOP LEFT
		}

		for dir, step := range moves {
			other := seat + step
			if other < 0 ||
				(dir < 3 && other%rows < seat%rows) ||
				(dir > 4 && other%rows > seat%rows) ||
				other >= len(layout) {
				continue
			}
			if see[dir] == '.' && (layout[other] == 'L' || layout[other] == '#') {
				see[dir] = layout[other]
			}
		}

		if bytes.Count(see, []byte{'.'}) == 0 {
			break
		}
	}

	return bytes.Count(see, []byte{'#'})
}
func file(name string) (layout []byte) {
	f, _ := os.Open(name)
	defer f.Close()

	s := bufio.NewScanner(f)
	for s.Scan() {
		layout = append(layout, s.Bytes()...)
	}
	return
}
func hash(input []byte) string {
	h := md5.New()
	h.Write(input)
	return hex.EncodeToString(h.Sum(nil))
}
func min(self int, other int) int {
	if other < self {
		return other
	}
	return self
}
func max(self int, other int) int {
	if other > self {
		return other
	}
	return self
}
