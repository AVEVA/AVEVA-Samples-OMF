package com.github.osisoft.omfapi;

import org.junit.Test;
import static org.junit.Assert.fail;

/**
 * Unit test for simple App.
 */
public class ProgramTest {
    /**
     * Rigorous Test :-)
     */
    @Test
    public void shouldAnswerWithTrue() {
        try {
            Program.toRun(true);
        } catch (Exception e) {
            fail(e.toString());
        }
    }
}
