package ClassLib065;

public class Class001 {
    public static String property() {
        return "ClassLib065" + " " + ClassLib006.Class001.property() + " " + ClassLib007.Class001.property() + " " + ClassLib056.Class001.property();
    }
}
