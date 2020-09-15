import unittest
from .program import main


class SampleTests(unittest.TestCase):

    @classmethod
    def test_main(cls):
        main(True)


if __name__ == "__main__":
    unittest.main()
