.PHONY: all build

all: build

build:
	apt-get install mono-core && cd src && mono ./labrat/updator/*.cs && make *
