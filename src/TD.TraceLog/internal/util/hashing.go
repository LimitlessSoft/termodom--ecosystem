package util

import (
	"encoding/hex"
	"hash/fnv"
)

func HashString(s string) string {
	h := fnv.New32()
	h.Write([]byte(s))
	return hex.EncodeToString(h.Sum(nil))
}
